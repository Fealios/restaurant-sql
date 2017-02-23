using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RestaurantApp.Objects
{
  public class Review
  {
    private int _id;
    private string _description;
    private int _restaurantId;
    private int _rating;

    public Review(string description, int restaurantId, int rating, int id=0)
    {
      _description = description;
      _restaurantId = restaurantId;
      _id = id;
      _rating = rating;
    }

    public int GetRevId()
    {
      return _id;
    }

    public string GetReview()
    {
      return _description;
    }

    public int GetRestId()
    {
      return _restaurantId;
    }

    public int GetRating()
    {
      return _rating;
    }

    public override bool Equals(System.Object otherReview)
    {
      if (!(otherReview is Review))
      {
        return false;
      }
      else
      {
        Review newReview = (Review) otherReview;
        bool revIdEquality = (this.GetRevId() == newReview.GetRevId());
        bool descEquality = (this.GetReview() == newReview.GetReview());
        bool restIdEquality = (this.GetRestId() == newReview.GetRestId());
        bool ratingEquality = (this.GetRating() == newReview.GetRating());
        return (restIdEquality && descEquality && restIdEquality && ratingEquality);
      }
    }

    public string GetRestName()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE id = @Restaurant_id;", conn);

      SqlParameter restNameParameter = new SqlParameter("@Restaurant_id", this.GetRestId());

      cmd.Parameters.Add(restNameParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      string restName = null;

      while(rdr.Read())
      {
        restName = rdr.GetString(1);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return restName;
    }

    public static List<Review> GetAll()
    {
      List<Review> allReviews = new List<Review>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM reviews;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int reviewId = rdr.GetInt32(0);
        string description = rdr.GetString(1);
        int restaurantId = rdr.GetInt32(2);
        int rating = rdr.GetInt32(3);
        Review newReview = new Review(description, restaurantId, rating, reviewId);
        allReviews.Add(newReview);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return allReviews;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO reviews (description, restaurant_id, ratings) OUTPUT INSERTED.id VALUES(@ReviewDescription, @Restaurant_Id, @Rating);", conn);

      SqlParameter reviewParameter = new SqlParameter("@ReviewDescription", this.GetReview());

      SqlParameter restaurantIdParameter = new SqlParameter("@Restaurant_Id", this.GetRestId());

      SqlParameter ratingParameter = new SqlParameter("@Rating", this.GetRating());

      cmd.Parameters.Add(reviewParameter);
      cmd.Parameters.Add(restaurantIdParameter);
      cmd.Parameters.Add(ratingParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    } // end save

    public static Review Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM reviews WHERE id = @ReviewId;", conn);
      SqlParameter reviewIdParameter = new SqlParameter("@ReviewId", id);
      cmd.Parameters.Add(reviewIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundReviewId = 0;
      string foundReviewDescription = null;
      int foundRestaurantId = 0;
      int foundRating = 0;

      while(rdr.Read())
      {
        foundReviewId = rdr.GetInt32(0);
        foundReviewDescription = rdr.GetString(1);
        foundRestaurantId = rdr.GetInt32(2);
        foundRating = rdr.GetInt32(3);
      }
      Review foundReview = new Review(foundReviewDescription, foundRestaurantId, foundRating, foundReviewId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundReview;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM reviews;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
