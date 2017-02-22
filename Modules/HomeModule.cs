using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using RestaurantApp.Objects;

namespace RestaurantApp
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Cuisine> AllCuisine = Cuisine.GetAll();
        return View["index.cshtml", AllCuisine];
      };

      Get["/cuisines"] =_=> {
        List<Cuisine> AllCuisine = Cuisine.GetAll();
        return View["cuisines.cshtml", AllCuisine];
      };

      Get["/add-cuisine"] =_=> {
        return View["add.cshtml"];
      };

      Get["/cuisine/{id}"] = parameter => {
        Cuisine indivCuisine = Cuisine.Find(parameter.id);
        return View["indivCuisine.cshtml", indivCuisine];
      };

      Post["/add-cuisine"] =_=> {
        Cuisine newCuisine = new Cuisine(Request.Form["cuisine"]);
        newCuisine.Save();

        return View["addConfirmation.cshtml", newCuisine.GetName()];
      };

      Get["/delete/confirmation/{id}"] = parameter => {
        Cuisine foundCuisine = Cuisine.Find(parameter.id);
        return View["deleteConfirmation.cshtml", foundCuisine];
      };

      Get["/cuisine/update/{id}"] = parameter =>
      {
        Cuisine foundCuisine = Cuisine.Find(parameter.id);
        return View["cuisineUpdate.cshtml", foundCuisine];
      };

      Patch["/cuisine/update/confirm/{id}"] = parameter =>
      {
        Cuisine foundCuisine = Cuisine.Find(parameter.id);
        foundCuisine.Update(Request.Form["cuisine-name"]);
        return View["cuisines.cshtml", Cuisine.GetAll()];
      };

      Post["/cuisine/add/{id}"] = parameter => {
        Restaurant newRestaurant = new Restaurant(Request.Form["restaurant-name"], parameter.id);
        newRestaurant.Save();
        return View["addRestaurantConfirmation.cshtml"];
      };

      Delete["/cuisine/delete/{id}"] = parameters =>
      {
        Cuisine deletedCuisine = Cuisine.Find(parameters.id);
        deletedCuisine.Delete();

        return View["successDelete.cshtml", deletedCuisine];
      };

      Get["/restaurant/{id}"] = parameter => {
        Restaurant tempRestaurant = Restaurant.Find(parameter.id);
        return View["indivRestaurant.cshtml", tempRestaurant];
      };

      Delete["/restaurant/delete/{id}"] =parameter=> {
        Restaurant foundRestaurant = Restaurant.Find(parameter.id);
        foundRestaurant.Delete();
        Cuisine foundCuisine = Cuisine.Find(foundRestaurant.GetCuisId());
        return View["indivCuisine.cshtml", foundCuisine];
      };

    }
  }
}
