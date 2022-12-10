using FixedAssetConsumeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;


namespace FixedAssetConsumeApi.Controllers
{
	public class FaclassController : Controller
	{
		string baseurl = "https://localhost:44308/";

		public async Task<IActionResult> Index()
		{
			List<TblFaClassEntity> listofTblFaclass=new List<TblFaClassEntity>();
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(baseurl);   // passing in the base url here
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // setting the header
				HttpResponseMessage getData = await client.GetAsync("/api/tblFaClass/GetTblFaclasses");

				if (getData.IsSuccessStatusCode)  // if we get staus code 200 sucessful code then 
				{
					string result = getData.Content.ReadAsStringAsync().Result;  // read as string to fetch us the json object
					listofTblFaclass = JsonConvert.DeserializeObject<List<TblFaClassEntity>>(result); // pass in data table here and esult
				}
				else
				{
					Console.WriteLine("Error in calling web api");
				}
				
			}
			return View(listofTblFaclass);
		}
		public async Task<ActionResult<string>> AddFaclass(TblFaClassEntity tblFaclass)
		{
			TblFaClassEntity obj = new TblFaClassEntity()
			{
				Id = tblFaclass.Id,
				CatCode = tblFaclass.CatCode,
				ClassCode = tblFaclass.ClassCode,
				ClassName = tblFaclass.ClassName,
				DepMethod = tblFaclass.DepMethod,
				DepRate = tblFaclass.DepRate,
				LifeSpan = tblFaclass.LifeSpan,
				Status = tblFaclass.Status,
				UserId = tblFaclass.UserId,
				AuthId = tblFaclass.AuthId

			};
			if (tblFaclass.CatCode != null)
			{
				using (var client = new HttpClient())
				{
					obj.CatCode = tblFaclass.CatCode;
					client.BaseAddress = new Uri(baseurl + " /api/tblFaClass/AddTblFaclass");   // passing in the base url here
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json")); // setting the header
					HttpResponseMessage adddata = await client.PostAsJsonAsync("/api/tblFaClass/AddTblFaclass", obj);// 

					if (adddata.IsSuccessStatusCode)  // if we get staus code 200 sucessful code then 
					{
						return RedirectToAction("Index", "Home");  // read as string to fetch us the json object

					}
					else
					{
						Console.WriteLine("Error in calling web api");
					}
					ViewData.Model = tblFaclass;
				}
			}

			return View();

		}

		public async Task<IActionResult> EditFaclass(int id)
		{
			TblFaClassEntity tblFaclass = new TblFaClassEntity();
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.BaseAddress = new Uri(baseurl);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType: "application/json"));


				var responseTask = await client.GetAsync("/api/tblFaClass/GetTblFaclassById"+id);
				if (responseTask.IsSuccessStatusCode)
				{
					var item = responseTask.Content.ReadAsStringAsync().Result;
					tblFaclass = JsonConvert.DeserializeObject<TblFaClassEntity>(item);
				}

				//create  a post method to update the data

			}
			return View(tblFaclass);
		}

		[HttpPost]
		public async Task<IActionResult> Update(TblFaClassEntity tblFaclass)
		{
			string CustomMessage;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Clear();
				client.BaseAddress = new Uri(baseurl);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType: "application/json"));
				var responseMessage = await client.PutAsJsonAsync("/api/tblFaCategory/UpdateTblFacategory" + tblFaclass.Id, tblFaclass);
				if (responseMessage.IsSuccessStatusCode)
				{
					var item = responseMessage.Content.ReadAsStringAsync().Result;
					CustomMessage = JsonConvert.DeserializeObject<string>(item);

				}


			}
			return View("Index");
		}

	}
}
