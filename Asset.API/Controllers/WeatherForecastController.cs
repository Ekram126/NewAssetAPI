using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.RequestVM;
using Asset.ViewModels.UserVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Asset.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IEmailSender _emailSender;

        private readonly IRequestService _requestService;




        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IEmailSender emailSender, IRequestService requestService)
        {
            _logger = logger;
            _emailSender = emailSender;
            _requestService = requestService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();

            //var message = new MessageVM(new string[] { "pineapple_126@hotmail.com" }, "Test email", "This is the content from our email.");
            //_emailSender.SendEmail(message);


            //const string from = "almostakbaltechnology.dev@gmail.com";
            //const string to = "pineapple_126@hotmail.com";
            //const string subject = "This is subject";
            //const string body = "This is body";
            //const string appSpecificPassword = "fajtjigwpcnxyyuv";

            //var mailMessage = new MailMessage(from, to, subject, body);
            //using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            //{
            //    smtpClient.EnableSsl = true;
            //    smtpClient.Credentials = new NetworkCredential(from, appSpecificPassword);
            //    smtpClient.Send(mailMessage);
            //}



            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }



        //[HttpPost]
        //[Route("SyncDB")]
        //public void SyncDB()
        //{
        //    //   var top5 = _requestService.GetAllRequests().ToList();




        //    SqlConnection con = new SqlConnection(@"Data Source=.;Initial Catalog=NewAssetDB;Integrated Security=true;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=True");
         
        //    SqlConnection con2 = new SqlConnection(@"Data Source=sql5109.site4now.net;user ID=db_a7c3c0_assetdb_admin;Password=P@ssw0rd;Initial Catalog=db_a7c3c0_assetdb;Integrated Security=false;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");



        //    // SqlCommand cmd = new SqlCommand("select top 5 * From Request", con);
        //    SqlCommand cmd = new SqlCommand("select * From Request Where Id=17", con);
        //    // SqlCommand cmd = new SqlCommand("select top 5 * From Request Where Id > 8", con);
        //    SqlCommand cmd2 = new SqlCommand("select * From Request", con2);
        //    con.Open();
        //    con2.Open();
        //    SqlDataReader dr = cmd.ExecuteReader();



        //    List<CreateRequestVM> hospitalRequests = new List<CreateRequestVM>();
        //    List<CreateRequestVM> centralRequests = new List<CreateRequestVM>();

        //    List<RequestTracking> lstTracks = new List<RequestTracking>();


        //    if (dr.HasRows)
        //    {
        //        while (dr.Read())
        //        {
        //            CreateRequestVM newItem = new CreateRequestVM();
        //            newItem.Id = dr.GetInt32(0);
        //            if (!dr.IsDBNull(1))
        //            {
        //                newItem.Subject = dr.GetString(1);
        //            }
        //            if (!dr.IsDBNull(2))
        //            {
        //                newItem.RequestCode = dr.GetString(2);
        //            }
        //            if (!dr.IsDBNull(3))
        //            {
        //                newItem.Description = dr.GetString(3);
        //            }
        //            newItem.RequestDate = dr.GetDateTime(4);
        //            newItem.RequestModeId = dr.GetInt32(5);
        //            if (!dr.IsDBNull(6))
        //            {
        //                newItem.SubProblemId = dr.GetInt32(6);
        //            }
        //            newItem.AssetDetailId = dr.GetInt32(7);
        //            newItem.RequestPeriorityId = dr.GetInt32(8);
        //            newItem.RequestTypeId = dr.GetInt32(9);
        //            newItem.CreatedById = dr.GetString(10);
        //            newItem.IsOpened = dr.GetBoolean(11);
        //            newItem.HospitalId = dr.GetInt32(12);
        //            hospitalRequests.Add(newItem);
        //        }
        //    }



        //    SqlDataReader dr2 = cmd2.ExecuteReader();
        //    if (dr2.HasRows)
        //    {
        //        while (dr2.Read())
        //        {
        //            CreateRequestVM newItem = new CreateRequestVM();
        //            newItem.Id = dr2.GetInt32(0);
        //            newItem.Subject = dr2.GetString(1);
        //            newItem.RequestCode = dr2.GetString(2);
        //            newItem.Description = dr2.GetString(3);
        //            newItem.RequestDate = dr2.GetDateTime(4);
        //            newItem.RequestModeId = dr2.GetInt32(5);
        //            //newItem.SubProblemId = dr2.GetInt32(6);
        //            //newItem.AssetDetailId = dr2.GetInt32(7);
        //            //newItem.RequestPeriorityId = dr2.GetInt32(8);
        //            //newItem.RequestTypeId = dr2.GetInt32(9);
        //            //newItem.CreatedById = dr2.GetString(10);
        //            //newItem.IsOpened = dr2.GetBoolean(11);
        //            newItem.HospitalId = dr2.GetInt32(12);
        //            centralRequests.Add(newItem);

        //        }
        //    }

        //    con.Close();
        //    con2.Close();

        //    //if (centralRequests.Count == 0)
        //    //{

        //    foreach (var item in hospitalRequests)
        //    {
        //        //   string query = @"INSERT INTO Request(Subject,RequestCode,Description,RequestDate,RequestModeId,SubProblemId,AssetDetailId,RequestPeriorityId,RequestTypeId,CreatedById,IsOpened,HospitalId) Values(@Subject,@RequestCode,@Description,@RequestDate,@RequestModeId,@SubProblemId,@AssetDetailId,@RequestPeriorityId,@RequestTypeId,@CreatedById,@IsOpened,@HospitalId)";


        //        using (var connection = new SqlConnection(@"Data Source=sql5109.site4now.net;user ID=db_a7c3c0_assetdb_admin;Password=P@ssw0rd;Initial Catalog=db_a7c3c0_assetdb;Integrated Security=false;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
        //        {
        //            connection.Open();
        //            var sql = "INSERT INTO Request(Subject,RequestCode,Description,RequestDate,RequestModeId,AssetDetailId,RequestPeriorityId,RequestTypeId,IsOpened,HospitalId) Values(@Subject,@RequestCode,@Description,@RequestDate,@RequestModeId,@AssetDetailId,@RequestPeriorityId,@RequestTypeId,@IsOpened,@HospitalId) SELECT SCOPE_IDENTITY()";
        //            // string idQuery = "Select SCOPE_IDENTITY()";



        //            using (var insertCMD = new SqlCommand(sql, connection))
        //            {
        //                if (item.Subject != null)
        //                    insertCMD.Parameters.AddWithValue("@Subject", item.Subject);
        //                else
        //                    insertCMD.Parameters.AddWithValue("@Subject", "");

        //                insertCMD.Parameters.AddWithValue("@RequestCode", item.RequestCode);


        //                if (item.Description != null)
        //                    insertCMD.Parameters.AddWithValue("@Description", item.Description);
        //                else
        //                    insertCMD.Parameters.AddWithValue("@Description", "");


        //                insertCMD.Parameters.AddWithValue("@RequestDate", item.RequestDate);




        //                if (item.RequestModeId != 0)
        //                    insertCMD.Parameters.AddWithValue("@RequestModeId", item.RequestModeId);
        //                else
        //                    insertCMD.Parameters.AddWithValue("@RequestModeId", 0);


        //                //if (item.SubProblemId != null)
        //                //    insertCMD.Parameters.AddWithValue("@SubProblemId", item.SubProblemId);
        //                //else
        //                //    insertCMD.Parameters.AddWithValue("@SubProblemId", 0);


        //                if (item.AssetDetailId != 0)
        //                    insertCMD.Parameters.AddWithValue("@AssetDetailId", item.AssetDetailId);
        //                else
        //                    insertCMD.Parameters.AddWithValue("@AssetDetailId", null);


        //                if (item.RequestPeriorityId != 0)
        //                    insertCMD.Parameters.AddWithValue("@RequestPeriorityId", item.RequestPeriorityId);
        //                else
        //                    insertCMD.Parameters.AddWithValue("@RequestPeriorityId", null);







        //                if (item.RequestTypeId != 0)
        //                    insertCMD.Parameters.AddWithValue("@RequestTypeId", item.RequestTypeId);
        //                else
        //                    insertCMD.Parameters.AddWithValue("@RequestTypeId", null);




        //                //if (item.CreatedById != null)
        //                //    insertCMD.Parameters.AddWithValue("@CreatedById", item.CreatedById);
        //                //else
        //                //insertCMD.Parameters.AddWithValue("@CreatedById", "");


        //                insertCMD.Parameters.AddWithValue("@IsOpened", item.IsOpened);
        //                insertCMD.Parameters.AddWithValue("@HospitalId", item.HospitalId);
        //                int returnId = (int)(decimal)insertCMD.ExecuteScalar();



        //                SqlCommand trackingcmd = new SqlCommand("select * From RequestTracking Where RequestId= 17", con);
        //                con.Open();
        //                SqlDataReader trackingreader = trackingcmd.ExecuteReader();
        //                if (trackingreader.HasRows)
        //                {
        //                    while (trackingreader.Read())
        //                    {
        //                        RequestTracking trackObj = new RequestTracking();
        //                        trackObj.Id = trackingreader.GetInt32(0);


        //                        if (!trackingreader.IsDBNull(1))
        //                        {
        //                            trackObj.Description = trackingreader.GetString(1);
        //                        }


        //                        trackObj.DescriptionDate = trackingreader.GetDateTime(2);
        //                        trackObj.RequestStatusId = trackingreader.GetInt32(3);
        //                        trackObj.RequestId = trackingreader.GetInt32(4);
        //                        trackObj.CreatedById = trackingreader.GetString(5);
        //                        trackObj.IsOpened = trackingreader.GetBoolean(6);
        //                        trackObj.HospitalId = trackingreader.GetInt32(7);
        //                        lstTracks.Add(trackObj);
        //                    }
        //                }


        //                if (lstTracks.Count > 0)
        //                {
        //                    foreach (var itm in lstTracks)
        //                    {
        //                        var sqlTracking = "INSERT INTO RequestTracking(Description,DescriptionDate,RequestStatusId,RequestId,IsOpened,HospitalId) Values(@Description,@DescriptionDate,@RequestStatusId,@RequestId,@IsOpened,@HospitalId)";
        //                        using (var insertTrackingCMD = new SqlCommand(sqlTracking, connection))
        //                        {
        //                            if (itm.Description != null)
        //                                insertTrackingCMD.Parameters.AddWithValue("@Description", itm.Description);
        //                            else
        //                                insertTrackingCMD.Parameters.AddWithValue("@Description", "");



        //                            insertTrackingCMD.Parameters.AddWithValue("@RequestStatusId", itm.RequestStatusId);
        //                            insertTrackingCMD.Parameters.AddWithValue("@DescriptionDate", itm.DescriptionDate);
        //                            insertTrackingCMD.Parameters.AddWithValue("@RequestId", returnId);
        //                            insertTrackingCMD.Parameters.AddWithValue("@IsOpened", itm.IsOpened);
        //                            insertTrackingCMD.Parameters.AddWithValue("@HospitalId", itm.HospitalId);
        //                            insertTrackingCMD.ExecuteNonQuery();
        //                        }

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    // }
        //}

    }
}
