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

    public Review(string description, int restaurantId, int id=0)
    {
      _description = description;
      _restaurantId = restaurantId;
      _id = id;
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
        return (restIdEquality && descEquality && restIdEquality);
      }
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
        Review newReview = new Review(description, restaurantId, reviewId);
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

      SqlCommand cmd = new SqlCommand("INSERT INTO reviews (description, restaurant_id) OUTPUT INSERTED.id VALUES(@ReviewDescription, @Restaurant_Id);", conn);

      SqlParameter reviewParameter = new SqlParameter("@ReviewDescription", this.GetReview());

      SqlParameter restaurantIdParameter = new SqlParameter("@Restaurant_Id", this.GetRestId());

      cmd.Parameters.Add(reviewParameter);
      cmd.Parameters.Add(restaurantIdParameter);

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
