using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace RestaurantApp.Objects
{
  public class Cuisine
  {
    private int _id;
    private string _name;

    public Cuisine(string name, int id = 0)
    {
      _id = id;
      _name = name;
    }

    public string GetName()
    {
      return _name;
    }

    public int GetId()
    {
      return _id;
    }

    public override bool Equals(System.Object otherCuisine)
    {
        if (!(otherCuisine is Cuisine))
        {
          return false;
        }
        else
        {
          Cuisine newCuisine = (Cuisine) otherCuisine;
          bool idEquality = this.GetId() == newCuisine.GetId();
          bool nameEquality = this.GetName() == newCuisine.GetName();
          return (idEquality && nameEquality);
        }
    }

    public static List<Cuisine> GetAll()
    {
      List<Cuisine> allCuisines = new List<Cuisine>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cuisines;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int CuisineId = rdr.GetInt32(0);
        string CuisineName = rdr.GetString(1);
        Cuisine newCuisine = new Cuisine(CuisineName, CuisineId);
        allCuisines.Add(newCuisine);
      }

      return allCuisines;
    }

    public static Cuisine Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cuisines WHERE id = @CuisineId;", conn);
      SqlParameter cuisineIdParameter = new SqlParameter("@CuisineId", id);
      cmd.Parameters.Add(cuisineIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCuisineId = 0;
      string foundCuisineName = null;

      while(rdr.Read())
      {
        foundCuisineId = rdr.GetInt32(0);
        foundCuisineName = rdr.GetString(1);
      }
      Cuisine foundCuisine = new Cuisine(foundCuisineName, foundCuisineId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCuisine;
    }

    public void Save()
   {
     SqlConnection conn = DB.Connection();
     conn.Open();

     SqlCommand cmd = new SqlCommand("INSERT INTO cuisines (name) OUTPUT INSERTED.id VALUES (@CuisineName);", conn);

     SqlParameter nameParameter = new SqlParameter();
     nameParameter.ParameterName = "@CuisineName";
     nameParameter.Value = this.GetName();
     cmd.Parameters.Add(nameParameter);
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
   }

   public List<Restaurant> GetRestaurants()
   {
     SqlConnection conn = DB.Connection();
     conn.Open();


     SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE cuisine_id = @Cuisine_Id;", conn);
     SqlParameter cuisineIdParameter = new SqlParameter("@Cuisine_Id", this.GetId());

     cmd.Parameters.Add(cuisineIdParameter);
     SqlDataReader rdr = cmd.ExecuteReader();

     List<Restaurant> allRestaurants = new List<Restaurant>{};
     while(rdr.Read())
     {
       int RestaurantId = rdr.GetInt32(0);
       string RestaurantName = rdr.GetString(1);
       int CuisineId = rdr.GetInt32(2);
       Restaurant newRestaurant = new Restaurant(RestaurantName, CuisineId, RestaurantId);

       allRestaurants.Add(newRestaurant);
     }
     if (rdr != null)
     {
       rdr.Close();
     }
     if (conn != null)
     {
       conn.Close();
     }
     return allRestaurants;
   }

   public void Update(string newName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE cuisines SET name = @NewName OUTPUT INSERTED.name WHERE id = @CuisineId;", conn);

      SqlParameter newNameParameter = new SqlParameter("@NewName", newName);
      cmd.Parameters.Add(newNameParameter);


      SqlParameter cuisineIdParameter = new SqlParameter();
      cuisineIdParameter.ParameterName = "@CuisineId";
      cuisineIdParameter.Value = this.GetId();
      cmd.Parameters.Add(cuisineIdParameter);
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

   public void Delete()
   {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM cuisines WHERE id = @CuisineId; DELETE FROM restaurants WHERE cuisine_id = @CuisineId;", conn);

      SqlParameter cuisineIdParameter = new SqlParameter();
      cuisineIdParameter.ParameterName = "@CuisineId";
      cuisineIdParameter.Value = this.GetId();

      cmd.Parameters.Add(cuisineIdParameter);
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
      SqlCommand cmd = new SqlCommand("DELETE FROM cuisines;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
