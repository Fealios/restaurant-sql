using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using RestaurantApp.Objects;

namespace RestaurantApp
{
  public class ReviewTest : IDisposable
  {
    public ReviewTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=yip_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_InputEqualsOutput()
    {
      //Arrange, Act
     int result = Review.GetAll().Count;

     //Assert
     Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
    {
      Review firstReview = new Review("This Blows", 2, 1);
      Review secondReview = new Review("This Blows", 2, 1);
    Assert.Equal(firstReview, secondReview);
    }

    [Fact]
    public void Test_Save_ReturnsSavedReview()
    {
      Review testReview = new Review("McDonalds", 2);
      testReview.Save();

      List<Review> totalReviews = Review.GetAll();
      List<Review> testReviews = new List<Review>{testReview};

      Assert.Equal(testReviews, totalReviews);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      Review testReview = new Review("this makes me want to retch", 2);

      testReview.Save();
      Review savedReview = Review.GetAll()[0];

      int result = savedReview.GetRestId();
      int testId = testReview.GetRestId();
      Assert.Equal(testId, result);
    }


    [Fact]
    public void Test_FindFindsReviewInDatabase()
    {
      //Arrange
      Review testReview = new Review("This restaurant is garbage", 2);
      testReview.Save();

      //Act
      Review foundReview = Review.Find(testReview.GetRevId());

      //Assert
      Assert.Equal(testReview, foundReview);
    }

    public void Dispose()
    {
      Review.DeleteAll();
    }

  }
}
