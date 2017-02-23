using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;
using RestaurantApp.Objects;
using System.Linq;

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

      Get["/restaurant/{id}/reviews"] = parameter => {
        Restaurant tempRestaurant = Restaurant.Find(parameter.id);
        List<Review> reviewList = Restaurant.GetReviews(parameter.id);
        Review restId = new Review("rest id", parameter.id, 4);
        List<Review> restIdList = new List<Review>{restId};

        Dictionary<string, List<Review>> passThrough = new Dictionary<string, List<Review>>{};
        passThrough.Add("reviews", reviewList);
        passThrough.Add("restId", restIdList);
        return View["reviewForm.cshtml", passThrough];
      };

      Post["/restaurant/{id}/reviews"] = parameter =>
      {
        Review newReview = new Review(Request.Form["review"], parameter.id, Request.Form["rating"]);
        newReview.Save();

        List<Review> reviewList = Restaurant.GetReviews(parameter.id);
        Review restId = new Review("rest id", parameter.id, 4);
        List<Review> restIdList = new List<Review>{restId};

        Dictionary<string, List<Review>> passThrough = new Dictionary<string, List<Review>>{};
        passThrough.Add("reviews", reviewList);
        passThrough.Add("restId", restIdList);

        return View["reviewForm.cshtml", passThrough];
      };

      Get["/all-restaurants"] = _ =>{
        List<Restaurant> allRestaurants = Restaurant.GetAll();
        var sortedList = allRestaurants.OrderBy(Restaurant => Restaurant.GetName());
        return View["all-restaurants.cshtml", sortedList];
      };


    }
  }
}
