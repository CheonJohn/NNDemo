using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace NN.Models
{
    public class RecipesModel
    {
        
        [JsonProperty(PropertyName = "recipes", Order = 10,  NullValueHandling = NullValueHandling.Ignore)]
        public List<Recipe> Recipes { get; set; }

        
        [JsonProperty(PropertyName = "message" , Order = 1 , NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
        

        public class Recipe
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public int? id { get; set; }
            public string title { get; set; }
            public string making_time { get; set; }
            public string serves { get; set; }
            public string ingredients { get; set; }
            public int cost { get; set; }

            [JsonIgnore]
            public DateTime created_at { get; set; }
            [JsonIgnore]
            public DateTime updated_at { get; set; }
        }

        private RecipesModel(List<Recipe> Recipes)
        {
            this.Recipes = Recipes;
        }


        public static RecipesModel Get()
        {
            return new RecipesModel(GetData());
        }

        public static RecipesModel Get(int id)
        {
            var response = new RecipesModel(GetData(id));

            
            response.Message = "Recipe details by id";

            if (response.Recipes.Count == 0)
            {
                response.Recipes = null;
                response.Message = "No Recipe found";

            }
            else
            {
                response.Recipes[0].id = null;
            }

            return response;
        }

        private enum DMLType
        {
            Insert,
            Update,
            Delete
        }

        public static int InsertData(Recipe recipe)
        {
            return DmlProcessing(DMLType.Insert, null, recipe.title, recipe.making_time, recipe.serves, recipe.ingredients, recipe.cost);
        }

        public static int UpdateData(Recipe recipe)
        {
            return DmlProcessing(DMLType.Update, recipe.id, recipe.title, recipe.making_time, recipe.serves, recipe.ingredients, recipe.cost);
        }

        public static int DeleteData(int id)
        {
            return DmlProcessing(DMLType.Delete, id, null, null, null, null, 0);
        }


        private static int DmlProcessing( DMLType dmlType, int?id , string title, string making_time, string serves, string ingredients, int cost)
        {
            using (SqlConnection connection = new SqlConnection( DB.ConnectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    if(dmlType==DMLType.Delete || dmlType == DMLType.Update)
                    {
                        command.Parameters.Add(new SqlParameter("id", (int)id));
                    }

                    if (dmlType == DMLType.Insert || dmlType == DMLType.Update)
                    {
                        command.Parameters.Add(new SqlParameter("title", title));
                        command.Parameters.Add(new SqlParameter("making_time", making_time));
                        command.Parameters.Add(new SqlParameter("serves", serves));
                        command.Parameters.Add(new SqlParameter("ingredients", ingredients));
                        command.Parameters.Add(new SqlParameter("cost", cost));
                        command.Parameters.Add(new SqlParameter("updated_at", DateTime.Now));
                        command.Parameters.Add(new SqlParameter("created_at", DateTime.Now));
                    }

                    // SQLの準備
                    if (dmlType == DMLType.Insert)
                    {
                        command.CommandText =
                        @"INSERT INTO dbo.recipes (title, making_time,serves,ingredients,cost,updated_at, created_at) 
                                                  OUTPUT INSERTED.id VALUES 
                                                  (@title, @making_time, @serves,@ingredients,@cost,@updated_at,@created_at)";
                        
                        var aa = command.ExecuteScalar();
                        return (int)aa;
                    }
                    else if (dmlType == DMLType.Delete)
                    {
                        command.CommandText =
                        @"DELETE FROM dbo.recipes where id = @id";
                    }
                    else
                    {
                        command.CommandText =
                       @"Update dbo.recipes Set 
                            title = @title, 
                            making_time = @making_time,
                            serves = @serves,
                            ingredients = @ingredients,
                            cost = @cost,
                            updated_at = @updated_at
                        where id = @id";
                    }
                   
           
                    // SQLの実行
                    return command.ExecuteNonQuery();
                    
                }
                
            }
        }

       
        private static List<Recipe> GetData(int? Id = null)
        {

            var returnData = new List<Recipe>();
            
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * ");
            sb.Append("FROM [dbo].[recipes]  ");

            if (Id != null) {
                sb.AppendLine("Where Id = " + Id); //IdはIntのため、SqlInject対策を省略
            }

            String sql = sb.ToString();
            using (SqlConnection connection = new SqlConnection( DB.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var recipe = new Recipe();
                            recipe.id = (int)reader["id"];
                            recipe.ingredients = (string)reader["ingredients"];
                            recipe.making_time = (string)reader["making_time"];
                            recipe.serves = (string)reader["serves"];
                            recipe.title = (string)reader["title"];
                            recipe.updated_at = (DateTime)reader["updated_at"];
                            recipe.cost = (int)reader["cost"];
                            recipe.created_at = (DateTime)reader["created_at"];

                            returnData.Add(recipe);
                        }
                    }
                }

            }

        
            return returnData;
            
        }
    }
}