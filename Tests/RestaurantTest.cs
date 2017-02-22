using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using RestaurantApp.Objects;

namespace RestaurantApp
{
  public class RestaurantTest : IDisposable
  {
    public RestaurantTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=yip_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_InputEqualsOutput()
    {
      //Arrange, Act
     int result = Restaurant.GetAll().Count;

     //Assert
     Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
    {
      Restaurant firstRestaurant = new Restaurant("Mcdonalds", 2, 1);
      Restaurant secondRestaurant = new Restaurant("Mcdonalds", 2, 1);
    Assert.Equal(firstRestaurant, secondRestaurant);
    }

    [Fact]
    public void Test_Save_ReturnsSavedRestaurant()
    {
      Restaurant testRestaurant = new Restaurant("McDonalds", 2);
      testRestaurant.Save();

      List<Restaurant> totalRestaurants = Restaurant.GetAll();
      List<Restaurant> testRestaurants = new List<Restaurant>{testRestaurant};

      Assert.Equal(testRestaurants, totalRestaurants);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      Restaurant testRestaurant = new Restaurant("Mcdonalds", 2);

      testRestaurant.Save();
      Restaurant savedRestaurant = Restaurant.GetAll()[0];

      int result = savedRestaurant.GetRestId();
      int testId = testRestaurant.GetRestId();
      Assert.Equal(testId, result);
    }


    [Fact]
    public void Test_FindFindsRestaurantInDatabase()
    {
      //Arrange
      Restaurant testRestaurant = new Restaurant("Wendy's", 2);
      testRestaurant.Save();

      //Act
      Restaurant foundRestaurant = Restaurant.Find(testRestaurant.GetRestId());

      //Assert
      Assert.Equal(testRestaurant, foundRestaurant);
    }

    public void Dispose()
    {
      Restaurant.DeleteAll();
    }

  }
}
