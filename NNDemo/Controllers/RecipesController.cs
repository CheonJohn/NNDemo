using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using NN.Models;

namespace NNDemo.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {

        [HttpGet]
        public ActionResult<RecipesModel> Get()
        {
            return RecipesModel.Get();
        }


        [HttpGet("{id}")]
        public ActionResult<RecipesModel> Get(int id)
        {
            return RecipesModel.Get(id);
        }


        [HttpPost]
        public ActionResult<RecipesModel> Post([FromBody]RecipesModel.Recipe recipe)
        {
           
            var getData = new RecipesModel();


            try
            {
                int newestId = RecipesModel.InsertData(recipe);
                getData = RecipesModel.Get(newestId);
                getData.Message = "Recipe successfully created!";
            }
            catch
            {

                getData.Recipes = null;
                getData.Message = "Recipe creation failed!";
                getData.RequiredMessage = "title, making_time, serves, ingredients, cost";

            }
            return getData;

        }


        [HttpPatch("{id}")]
        public ActionResult<RecipesModel> Patch(int id, [FromBody]RecipesModel.Recipe recipe)
        {
            recipe.id = id;

            RecipesModel.UpdateData(recipe);

            var getData = RecipesModel.Get(id);

            getData.Message = "Recipe successfully updated!";

            return getData;
        }


        [HttpDelete("{id}")]
        public ActionResult<RecipesModel> Delete(int id)
        {
            RecipesModel.DeleteData(id);

            var getData = RecipesModel.Get(id);

            getData.Message = "Recipe successfully removed!";

            return getData;
        }
    }
}
