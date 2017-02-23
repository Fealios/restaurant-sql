using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RestaurantApp.Objects
{
  public class Restaurant
  {
    private string _name;
    private int _cuisineId;
    private int _restaurantId;

    public Restaurant(string name, int Cuisineid, int Restaurantid =0)
    {
      _name = name;
      _cuisineId = Cuisineid;
      _restaurantId = Restaurantid;
    }

    public string GetName()
    {
      return _name;
    }

    public int GetRestId()
    {
      return _restaurantId;
    }

    public int GetCuisId()
    {
      return _cuisineId;
    }

    public override bool Equals(System.Object otherRestaurant)
    {
      if (!(otherRestaurant is Restaurant))
      {
        return false;
      }
      else
      {
        Restaurant newRestaurant = (Restaurant) otherRestaurant;
        bool restIdEquality = (this.GetRestId() == newRestaurant.GetRestId());
        bool nameEquality = (this.GetName() == newRestaurant.GetName());
        bool cuisIdEquality = (this.GetCuisId() == newRestaurant.GetCuisId());
        return (restIdEquality && nameEquality && cuisIdEquality);
      }
    }

    public static List<Restaurant> GetAll()
    {
      List<Restaurant> allRestaurants = new List<Restaurant>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int restaurantId = rdr.GetInt32(0);
        string restaurantName = rdr.GetString(1);
        int restaurantCuisId = rdr.GetInt32(2);
        Restaurant newRestaurant = new Restaurant(restaurantName,restaurantCuisId, restaurantId);
        allRestaurants.Add(newRestaurant);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
      return allRestaurants;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM restaurants WHERE id = @RestaurantId;", conn);

      SqlParameter restaurantIdParameter = new SqlParameter();
      restaurantIdParameter.ParameterName = "@RestaurantId";
      restaurantIdParameter.Value = this.GetRestId();

      cmd.Parameters.Add(restaurantIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM restaurants;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO restaurants(name, cuisine_id) OUTPUT INSERTED.id VALUES(@RestaurantName, @Cuisine_Id);", conn);

      SqlParameter nameParameter = new SqlParameter("@RestaurantName", this.GetName());
      // nameParameter.ParameterName = "@RestaurantName";
      // nameParameter.Value = this.GetName();

      SqlParameter cuisineIdParameter = new SqlParameter();
      cuisineIdParameter.ParameterName = "@Cuisine_Id";
      cuisineIdParameter.Value = this.GetCuisId();

      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(cuisineIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._restaurantId = rdr.GetInt32(0);
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

    public static Restaurant Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE id = @Restaurant_Id;", conn);

      SqlParameter restaurantIdParameter = new SqlParameter("@Restaurant_Id", id);
      cmd.Parameters.Add(restaurantIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      int foundRestaurantId = 0;
      string foundRestaurantName = null;
      int foundRestaurantCuisinId = 0;

      while(rdr.Read())
      {
        foundRestaurantId = rdr.GetInt32(0);
        foundRestaurantName = rdr.GetString(1);
        foundRestaurantCuisinId = rdr.GetInt32(2);
      }

      Restaurant foundRestaurant = new Restaurant(foundRestaurantName, foundRestaurantCuisinId, foundRestaurantId);
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundRestaurant;

    }

    public void Update(string newName)
     {
       SqlConnection conn = DB.Connection();
       conn.Open();

       SqlCommand cmd = new SqlCommand("UPDATE restaurants SET name = @NewName OUTPUT INSERTED.name WHERE id = @RestaurantId;", conn);

       SqlParameter newNameParameter = new SqlParameter("@NewName", newName);
       cmd.Parameters.Add(newNameParameter);


       SqlParameter restaurantIdParameter = new SqlParameter();
       restaurantIdParameter.ParameterName = "@RestaurantId";
       restaurantIdParameter.Value = this.GetRestId();
       cmd.Parameters.Add(restaurantIdParameter);
       SqlDataReader rdr = cmd.ExecuteReader();

       while(rdr.Read())
       {
         this._name = rdr.GetString(0);
       }

       if (rdr != null)
       {
         rdr.Close();
       }

       if (conn != null)
       {
         conn.Close();
       }
     }


  }
}
