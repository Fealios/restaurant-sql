using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using RestaurantApp.Objects;

namespace RestaurantApp
{
  public class CuisineTest : IDisposable
  {
    public CuisineTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=yip_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CuisineEmptyAtFirst()
    {
      //Arrange, Act
      int result = Cuisine.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
   public void Test_CuisinesEmptyAtFirst()
   {
     //Arrange, Act
     int result = Cuisine.GetAll().Count;

     //Assert
     Assert.Equal(0, result);
   }

   [Fact]
   public void Test_Equal_ReturnsTrueForSameName()
   {
     //Arrange, Act
     Cuisine firstCuisine = new Cuisine("Italian");
     Cuisine secondCuisine = new Cuisine("Italian");

     //Assert
     Assert.Equal(firstCuisine, secondCuisine);
   }

   [Fact]
    public void Test_Save_SavesCuisineToDatabase()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Japanese");
      testCuisine.Save();

      //Act
      List<Cuisine> result = Cuisine.GetAll();
      List<Cuisine> testList = new List<Cuisine>{testCuisine};

      //Assert
      Assert.Equal(testList, result);
    }

   [Fact]
    public void Test_Save_AssignsIdToCuisineObject()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Italian");
      testCuisine.Save();

      //Act
      Cuisine savedCuisine = Cuisine.GetAll()[0];

      int result = savedCuisine.GetId();
      int testId = testCuisine.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCuisineInDatabase()
    {
      //Arrange
      Cuisine testCuisine = new Cuisine("Italian");
      testCuisine.Save();

      //Act
      Cuisine foundCuisine = Cuisine.Find(testCuisine.GetId());

      //Assert
      Assert.Equal(testCuisine, foundCuisine);
    }

    [Fact]
    public void Test_GetRestaurants_RetrievesAllRestaurantsWithCuisine()
    {
      Cuisine testCuisine = new Cuisine("Italian");
      testCuisine.Save();

      Restaurant firstRestaurant = new Restaurant("Olive Garden", testCuisine.GetId());
      firstRestaurant.Save();
      Restaurant secondRestaurant = new Restaurant("Spaghetti Factory", testCuisine.GetId());
      secondRestaurant.Save();


      List<Restaurant> testRestaurantList = new List<Restaurant> {firstRestaurant, secondRestaurant};
      List<Restaurant> resultRestaurantList = testCuisine.GetRestaurants();

      Assert.Equal(testRestaurantList, resultRestaurantList);
    }

    [Fact]
    public void Test_Delete_DeletesCuisineFromDatabase()
    {
      //Arrange
      string name1 = "Italian";
      Cuisine testCuisine1 = new Cuisine(name1);
      testCuisine1.Save();

      string name2 = "French";
      Cuisine testCuisine2 = new Cuisine(name2);
      testCuisine2.Save();

      Restaurant testRestaurant1 = new Restaurant("Mow the lawn", testCuisine1.GetId());
      testRestaurant1.Save();
      Restaurant testRestaurant2 = new Restaurant("Send emails", testCuisine2.GetId());
      testRestaurant2.Save();

      //Act
      testCuisine1.Delete();
      List<Cuisine> resultCuisine = Cuisine.GetAll();
      List<Cuisine> testCuisineList = new List<Cuisine> {testCuisine2};

      List<Restaurant> resultRestaurants = Restaurant.GetAll();
      List<Restaurant> testRestaurantList = new List<Restaurant> {testRestaurant2};

      //Assert
      Assert.Equal(testCuisineList, resultCuisine);
      Assert.Equal(testRestaurantList, resultRestaurants);
    }

   public void Dispose()
   {
     Cuisine.DeleteAll();
     Restaurant.DeleteAll();
   }

  }
}
