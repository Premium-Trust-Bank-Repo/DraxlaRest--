using Domain;
using DraxlaRest.API;
using DraxlaRest.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using VacmanBoundContext;
using VacmanBoundRepo;
using Vasco;

namespace DraxlaRest.Controllers
{
    public class DraxlaConnectController : ApiController
    {


        VacmanContext context = new VacmanContext();
        private readonly VacmanRepoUOW.DigipassRepo _repoDigipass;
       // private readonly VacmanRepoUOW.TInstanceRepo _repoInstance;


        ResponseObj respobj = new ResponseObj();
        List<TokenDetails> resp = new List<TokenDetails>();
        string respMsg = "", respCode = "", token= "";
        static AAL2Wrap dpxExplore;


        public DraxlaConnectController()
        {

            ObjUtil.WriteErrorLog("isTokenLock11111AAAAAAAAAAAUUUU",
 "threshod: " + "" + Environment.NewLine + "************" + Environment.NewLine +
  "count: " + "" + "" + Environment.NewLine + "***********" + Environment.NewLine + "");


            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
            _repoDigipass = new VacmanRepoUOW.DigipassRepo();
           // _repoInstance = new VacmanRepoUOW.TInstanceRepo();
        }



        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.Route("api/DraxlaConnect/IASAuthenticateUserOTP/{Domain}/{UserName}/{OTP}")]
        public ResponseObj IASAuthenticateUserOTP(string Domain,string UserName,string OTP)
        {
            string returnMsg = "";  // ReturnCode|token_serial_number|ActivationCode
            respobj = new ResponseObj();
            respMsg = "";
            respCode = "";

            string soap = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
            xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
               <soap:Body>
                 <AuthoriseUser xmlns=""http://tempuri.org/"">
                     <domain>" + Domain    + "</domain> " +
                       "<user_id>" + UserName + "</user_id> " +
                   "<dpResponse>" + OTP + "</dpResponse>" +
             "</AuthoriseUser>" +
            "</soap:Body>" +
       "</soap:Envelope>";



            returnMsg = DraxlaCallOTP(soap);

            string textXml = returnMsg;
            XElement xmlroot = XElement.Parse(textXml);
            string firstNodeContent = ((System.Xml.Linq.XElement)(xmlroot.FirstNode)).Value;

            string[] split = firstNodeContent.Split('~');

            respobj.RespCode = split[0];
            respobj.RespMsg = split[1];
            return respobj;
        }




        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.Route("api/DraxlaConnect/IASAuthenticateUserOTPPWD/{Domain}/{UserName}/{PWDOTP}")]
        public ResponseObj IASAuthenticateUserOTPPWD(string Domain, string UserName, string PWDOTP)
        {
            string returnMsg = "";  // ReturnCode|token_serial_number|ActivationCode
            respobj = new ResponseObj();
            respMsg = "";
            respCode = "";

            string soap = @"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
            xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
               <soap:Body>
                 <AuthoriseUser xmlns=""http://tempuri.org/"">
                     <domain>" + Domain + "</domain> " +
                         "<userID>" + UserName + "</userID> " +
                     "<dpResponse>" + PWDOTP + "</dpResponse>" +
               "</AuthoriseUser>" +
              "</soap:Body>" +
         "</soap:Envelope>";

            returnMsg = DraxlaCallOTPPWD(soap);

            string textXml = returnMsg;
            XElement xmlroot = XElement.Parse(textXml);
            string firstNodeContent = ((System.Xml.Linq.XElement)(xmlroot.FirstNode)).Value;

            string[] split = firstNodeContent.Split('-');

            respobj.RespCode = split[0];
            respobj.RespMsg = split[1];
            return respobj;
        }


        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.Route("api/DraxlaConnect/upload")]
        public returnDigipassJson Upload(upload filedetails)
        {
            returnDigipassJson resp = null;
            returnDigipassJson hg = new returnDigipassJson();
            List<digipass> gh = new List<digipass>();


            try
            {
                if (filedetails.filename == "" || filedetails.transportkey == "")
                {
                    hg = new returnDigipassJson();
                    hg.status = "1";
                    hg.message = "File name and Transport key must be provided";
                    hg.data = null;
                }
                else
                {
                    string msg = string.Empty;
                    string tkcount = "";
                    string filepath = ConfigurationManager.AppSettings["filepath"];

                    if (!Directory.Exists(filepath))
                    {
                        hg = new returnDigipassJson();
                        hg.status = "1";
                        hg.message = "File does not exist on the server";
                        hg.data = null;
                    }
                    else
                    {
                        hg = CommitDpxFile(filepath + "\\" + filedetails.filename, filedetails.transportkey, filedetails.uploader, ref msg, ref tkcount);

                        /*
                        if (resp.ToString().Contains("[ ERROR ]  Master Key content is incorrect"))
                        {
                            //resp = "[ ERROR ]  Master Key content is incorrect";
                        }
                        else
                        {
                            // resp = resp.ToString() + "|" + saveLocation;
                        }

                        */
                    }



                }



            }
            catch (Exception ex)
            {
                ObjUtil.WriteErrorLog("DPX UPLOAD",
                            ex.Message + Environment.NewLine + "************" + Environment.NewLine +
                            ex.StackTrace + Environment.NewLine + "***********" + Environment.NewLine + ex.InnerException);

                // resp = "Operation not successful due to an error. Contact your system administrator";


            }

            return hg;

        }



        //[System.Web.Http.AcceptVerbs("POST")]
        //[System.Web.Http.Route("api/DraxlaConnect/enumerateRetailUser")]
        //public UserEnumeration EnumerateRetailUser(Authentication auth)
        //{


        //    ObjUtil.WriteErrorLog("EnumerateRetailUser",
        //       "Resp endpoint11111111111AAAAAAAAA:" + "mmmmmm" + Environment.NewLine + "****" + Environment.NewLine +
        //         "" + Environment.NewLine + "*****" + Environment.NewLine);


        //    //if (auth.sno == null)
        //    //{
        //    //    auth.sno = "0909090909";
        //    //}


        //    UserEnumeration userenum = new UserEnumeration();
        //    string apiurl = ConfigurationManager.AppSettings["globusRetailEnumeration"].ToString(CultureInfo.InvariantCulture);

        //    string resp = "";
        //    Random rx = new Random();
        //    int rand = rx.Next(10001, 20000);
        //    string tran_id = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + '-' + rand;

        //    //Encryption ect = new Encryption();
        //    //string UserId = ect.Encrypt(auth.userid);
        //    //string TranId = ect.Encrypt(tran_id);


        //    ObjUtil.WriteErrorLog("EnumerateRetailUser",
        //       "Resp endpoint222222222222:" + "mmmmmm" + Environment.NewLine + "****" + Environment.NewLine +
        //         "" + Environment.NewLine + "*****" + Environment.NewLine);




        //    //if (token.Trim() == "")
        //    //{
        //    //    token = GetToken();
        //    //}

        //    ObjUtil.WriteErrorLog("EnumerateRetailUser",
        //       "Resp endpoint33333333333:" + "mmmmmm" + Environment.NewLine + "****" + Environment.NewLine +
        //         "token: " + token + " " + Environment.NewLine + "*****" + Environment.NewLine);


        //    ObjUtil.WriteErrorLog("EnumerateRetailUser",
        //       "Resp endpoint444444:" + token + Environment.NewLine + "****" + Environment.NewLine +
        //         "" + Environment.NewLine + "*****" + Environment.NewLine);




        //    //if (token.Trim() == "")
        //    //{
        //    //    userenum = new UserEnumeration()
        //    //    {
        //    //        userID = "",
        //    //        sno = "",
        //    //        customerFirstName = "",
        //    //        customerLastName = "",
        //    //        customerMiddleName = "",
        //    //        customerEmail = "",
        //    //        customerPhoneNo = "",
        //    //        responseCode = "-1",
        //    //        customerMsg = "Token cannot be retrieved at this time. Please retry."

        //    //    };

        //    //    return userenum;

        //    //}
        //    //else
        //    //{


        //        try
        //        {
        //            ObjUtil.WriteErrorLog("EnumerateRetailUser",
        //          "Resp endpoint555555 today:" + token + Environment.NewLine + "****" + Environment.NewLine +
        //            "" + Environment.NewLine + "*****" + Environment.NewLine);



        //            ServicePointManager.Expect100Continue = true;
        //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
        //           (se, cert, chain, sslerror) =>
        //           {
        //               return true;
        //           };

        //        //string corporateId = auth.accesscode;
        //        //string corpId = corporateId.Replace("_", "");
        //        //apiurl = apiurl + corpId;


        //        apiurl = apiurl + auth.userid;
        //        var client = new RestClient(apiurl);
        //        // var client = new RestClient(apiurl + auth.userid + "&" + "custId=" + auth.accesscode);
        //        client.Timeout = -1;
        //            var request = new RestRequest(Method.GET);
        //           // request.AddHeader("Authorization", "Bearer " + token);
        //            var body = @"";
        //            request.AddParameter("text/plain", body, ParameterType.RequestBody);
        //            IRestResponse response = client.Execute(request);

        //            var hj = response.Content;
        //            JavaScriptSerializer js = new JavaScriptSerializer();
        //            var myresponse = js.Deserialize<dynamic>(hj);





        //            ObjUtil.WriteErrorLog("EnumerateRetailUser",
        //       "Resp endpoint55555555 today2:" + "vvvvvv" + Environment.NewLine + "****" + Environment.NewLine +
        //         "" + Environment.NewLine + "*****" + Environment.NewLine);

        //            //var ret = myresponse["data"];
        //            string respCode = myresponse["responseCode"];
        //            string respMsg = myresponse["responseMessage"];

        //            ObjUtil.WriteErrorLog("EnumerateRetailUser",
        //          "Call endpoint: " + apiurl + Environment.NewLine + "****" + Environment.NewLine +
        //            "" + Environment.NewLine + "*****" + Environment.NewLine);


        //            ObjUtil.WriteErrorLog("EnumerateRetailUser",
        //            "Resp endpointvvv:" + hj.ToString() + Environment.NewLine + "****" + Environment.NewLine +
        //              "" + Environment.NewLine + "*****" + Environment.NewLine);



        //            if (myresponse["result"] != null)
        //            {
        //                string fullname = myresponse["result"]["lastName"] + " " + myresponse["result"]["firstName"] + " " + myresponse["result"]["middleName"];
        //                // fullname = fullname.Replace(",", " ");


        //                ObjUtil.WriteErrorLog("EnumerateRetailUser",
        //           "Resp endpoint66666 todayno:" + fullname + Environment.NewLine + "****" + Environment.NewLine +
        //             "" + Environment.NewLine + "*****" + Environment.NewLine);


        //                if(respCode == "00")
        //                {
        //                    respCode = "0";
        //                }
        //                //string sms = SendSMSAfterEnumeration(myresponse["result"]["registeredEmail"], "");

        //                //string[] split = sms.Split('|');

        //                //if(split[0] != "0")
        //                //{
        //                //    userenum = new UserEnumeration()
        //                //    {
        //                //        userID = "",
        //                //        sno = "",
        //                //        customerFirstName = "",
        //                //        customerLastName = "",
        //                //        customerMiddleName = "",
        //                //        customerEmail = "",
        //                //        customerPhoneNo = "",
        //                //        responseCode = "1",
        //                //        customerMsg = "SMS OTP sending failed"

        //                //    };
        //                //}
        //                //else
        //                //{
        //                    userenum = new UserEnumeration()
        //                    {

        //                        //userID = auth.userid,
        //                        // userID = myresponse["userId"],
        //                        //userID = myresponse["data"]["userId"],
        //                        userID = myresponse["result"]["userId"],
        //                        sno = auth.sno,
        //                        customerFullname = fullname,

        //                       // customerFirstName = split[1],
        //                        customerFirstName =  myresponse["result"]["firstName"],
        //                        //customerFirstName = myresponse["data"]["customerFirstName"],
        //                        //customerLastName = myresponse["data"]["customerLastName"],
        //                        //customerMiddleName = myresponse["data"]["customerLastName"],
        //                        customerEmail = myresponse["result"]["registeredEmailAddress"],
        //                        customerPhoneNo = myresponse["result"]["registeredPhoneNumber"],
        //                        responseCode = respCode,
        //                        customerMsg = respMsg






        //                    };
        //                //}




        //            }
        //            else
        //            {
        //                userenum = new UserEnumeration()
        //                {
        //                    userID = "",
        //                    sno = "",
        //                    customerFirstName = "",
        //                    customerLastName = "",
        //                    customerMiddleName = "",
        //                    customerEmail = "",
        //                    customerPhoneNo = "",
        //                    responseCode = respCode.ToString(),
        //                    customerMsg = respMsg

        //                };
        //            }

        //            ObjUtil.WriteErrorLog("EnumerateRetailUser",
        //           "Resp endpoint77777:" + "" + Environment.NewLine + "****" + Environment.NewLine +
        //             "" + Environment.NewLine + "*****" + Environment.NewLine);


        //        }
        //        catch (Exception ex)
        //        {

        //            ObjUtil.WriteErrorLog("EnumerateRetailUser error",
        //               "Error:" + ex.ToString() + Environment.NewLine + "****" + Environment.NewLine +
        //                 "" + Environment.NewLine + "*****" + Environment.NewLine);

        //            userenum = new UserEnumeration()
        //            {

        //                //userID = auth.userid,
        //                userID = "",
        //                sno = "",
        //                customerFullname = "",
        //                customerEmail = "",
        //                customerPhoneNo = "",
        //                responseCode = "1",
        //                customerMsg = "An error occured on enumerating"

        //            };



        //        }



        //        //userenum = new UserEnumeration()
        //        //{
        //        //    userID = auth.userid,
        //        //    sno = auth.sno,
        //        //    customerFirstName = "Feyisara",
        //        //    customerLastName = "Taiwo",
        //        //    customerMiddleName = "Kemi",
        //        //    customerFullname = "Taiwo Feyisara Kemi",
        //        //    customerEmail = "feyisara@maxut.com",
        //        //    customerPhoneNo = "09098887656",
        //        //    responseCode = "0",
        //        //    customerMsg = "Operation successful"

        //        //};

        //    //}



        //    auth = null;

        //    ObjUtil.WriteErrorLog("XXXXXXXXXXXXXXXXX",
        //               "User id:" + userenum.userID + Environment.NewLine + "****" + Environment.NewLine +
        //                 "Response code: " + userenum.responseCode + " " + Environment.NewLine + "***** " + "Response Msg: " + userenum.customerMsg + " ");



        //    return userenum;
        //}


        
        [System.Web.Http.AcceptVerbs("POST")] // Polaris
        [System.Web.Http.Route("api/DraxlaConnect/enumerateRetailUser")]
        public UserEnumeration EnumerateRetailUser(Authentication auth)
        {


            ObjUtil.WriteErrorLog("EnumerateRetailUser",
               "Resp endpoint11111111111AAAAAAAAA:" + "mmmmmm" + Environment.NewLine + "****" + Environment.NewLine +
                 "" + Environment.NewLine + "*****" + Environment.NewLine);


            //if (auth.sno == null)
            //{
            //    auth.sno = "0909090909";
            //}


            UserEnumeration userenum = new UserEnumeration();
            string apiurl = ConfigurationManager.AppSettings["polarisRetailEnumeration"].ToString(CultureInfo.InvariantCulture);

            string resp = "";
            Random rx = new Random();
            int rand = rx.Next(10001, 20000);
            string tran_id = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + '-' + rand;

            //Encryption ect = new Encryption();
            //string UserId = ect.Encrypt(auth.userid);
            //string TranId = ect.Encrypt(tran_id);


            ObjUtil.WriteErrorLog("EnumerateRetailUser",
               "Resp endpoint222222222222:" + "mmmmmm" + Environment.NewLine + "****" + Environment.NewLine +
                 "" + Environment.NewLine + "*****" + Environment.NewLine);




            //if (token.Trim() == "")
            //{
            //    token = GetToken();
            //}

            ObjUtil.WriteErrorLog("EnumerateRetailUser",
               "Resp endpoint33333333333:" + "mmmmmm" + Environment.NewLine + "****" + Environment.NewLine +
                 "token: " + token + " " + Environment.NewLine + "*****" + Environment.NewLine);


            ObjUtil.WriteErrorLog("EnumerateRetailUser",
               "Resp endpoint444444:" + token + Environment.NewLine + "****" + Environment.NewLine +
                 "" + Environment.NewLine + "*****" + Environment.NewLine);




            //if (token.Trim() == "")
            //{
            //    userenum = new UserEnumeration()
            //    {
            //        userID = "",
            //        sno = "",
            //        customerFirstName = "",
            //        customerLastName = "",
            //        customerMiddleName = "",
            //        customerEmail = "",
            //        customerPhoneNo = "",
            //        responseCode = "-1",
            //        customerMsg = "Token cannot be retrieved at this time. Please retry."

            //    };

            //    return userenum;

            //}
            //else
            //{


            try
            {
                ObjUtil.WriteErrorLog("EnumerateRetailUser",
              "Resp endpoint555555 today:" + token + Environment.NewLine + "****" + Environment.NewLine +
                "" + Environment.NewLine + "*****" + Environment.NewLine);



                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
               (se, cert, chain, sslerror) =>
               {
                   return true;
               };

                //string corporateId = auth.accesscode;
                //string corpId = corporateId.Replace("_", "");
                //apiurl = apiurl + corpId;


                apiurl = apiurl + auth.accesscode + "&accountNumber=" + auth.userid; //post request
                var client = new RestClient(apiurl);
                // var client = new RestClient(apiurl + auth.userid + "&" + "custId=" + auth.accesscode);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);     
                // request.AddHeader("Authorization", "Bearer " + token);
                var body = @"";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                var hj = response.Content;
                JavaScriptSerializer js = new JavaScriptSerializer();
                var myresponse = js.Deserialize<dynamic>(hj);





                ObjUtil.WriteErrorLog("EnumerateRetailUser",
           "Resp endpoint55555555 today2:" + "vvvvvv" + Environment.NewLine + "****" + Environment.NewLine +
             "" + Environment.NewLine + "*****" + Environment.NewLine);

                //var ret = myresponse["data"];
                string respCode = myresponse["responseCode"];
                string respMsg = myresponse["responseMessage"];

                ObjUtil.WriteErrorLog("EnumerateRetailUser",
              "Call endpoint: " + apiurl + Environment.NewLine + "****" + Environment.NewLine +
                "" + Environment.NewLine + "*****" + Environment.NewLine);


                ObjUtil.WriteErrorLog("EnumerateRetailUser",
                "Resp endpointvvv:" + hj.ToString() + Environment.NewLine + "****" + Environment.NewLine +
                  "" + Environment.NewLine + "*****" + Environment.NewLine);



                if (myresponse["data"] != null)
                {
                    string fullname = myresponse["data"]["lastName"] + " " + myresponse["data"]["firstName"] + " " + myresponse["data"]["middleName"];
                    // fullname = fullname.Replace(",", " ");


                    ObjUtil.WriteErrorLog("EnumerateRetailUser",
               "Resp endpoint66666 todayno:" + fullname + Environment.NewLine + "****" + Environment.NewLine +
                 "" + Environment.NewLine + "*****" + Environment.NewLine);


                    if (respCode == "00")
                    {
                        respCode = "0";
                    }
                    //string sms = SendSMSAfterEnumeration(myresponse["result"]["registeredEmail"], "");

                    //string[] split = sms.Split('|');

                    //if(split[0] != "0")
                    //{
                    //    userenum = new UserEnumeration()
                    //    {
                    //        userID = "",
                    //        sno = "",
                    //        customerFirstName = "",
                    //        customerLastName = "",
                    //        customerMiddleName = "",
                    //        customerEmail = "",
                    //        customerPhoneNo = "",
                    //        responseCode = "1",
                    //        customerMsg = "SMS OTP sending failed"

                    //    };
                    //}
                    //else
                    //{
                    userenum = new UserEnumeration()
                    {
                        //were the changes took place
                        //userID = auth.userid,
                        // userID = myresponse["userId"],
                        //userID = myresponse["data"]["userId"],
                        userID = myresponse["data"]["username"], //
                        sno = auth.sno,
                        customerFullname = fullname,

                        // customerFirstName = split[1],
                        customerFirstName = myresponse["data"]["firstName"],
                        //customerFirstName = myresponse["data"]["customerFirstName"],
                        //customerLastName = myresponse["data"]["customerLastName"],
                        //customerMiddleName = myresponse["data"]["customerLastName"],
                        customerEmail = myresponse["data"]["email"],
                        customerPhoneNo = myresponse["data"]["phoneNumber"],
                        responseCode = respCode,
                        customerMsg = respMsg






                    };
                    //}




                }
                else
                {
                    userenum = new UserEnumeration()
                    {
                        userID = "",
                        sno = "",
                        customerFirstName = "",
                        customerLastName = "",
                        customerMiddleName = "",
                        customerEmail = "",
                        customerPhoneNo = "",
                        responseCode = respCode.ToString(),
                        customerMsg = respMsg

                    };
                }

                ObjUtil.WriteErrorLog("EnumerateRetailUser",
               "Resp endpoint77777:" + "" + Environment.NewLine + "****" + Environment.NewLine +
                 "" + Environment.NewLine + "*****" + Environment.NewLine);


            }
            catch (Exception ex)
            {

                ObjUtil.WriteErrorLog("EnumerateRetailUser error",
                   "Error:" + ex.ToString() + Environment.NewLine + "****" + Environment.NewLine +
                     "" + Environment.NewLine + "*****" + Environment.NewLine);

                userenum = new UserEnumeration()
                {

                    //userID = auth.userid,
                    userID = "",
                    sno = "",
                    customerFullname = "",
                    customerEmail = "",
                    customerPhoneNo = "",
                    responseCode = "1",
                    customerMsg = "An error occured on enumerating"

                };



            }



            //userenum = new UserEnumeration()
            //{
            //    userID = auth.userid,
            //    sno = auth.sno,
            //    customerFirstName = "Feyisara",
            //    customerLastName = "Taiwo",
            //    customerMiddleName = "Kemi",
            //    customerFullname = "Taiwo Feyisara Kemi",
            //    customerEmail = "feyisara@maxut.com",
            //    customerPhoneNo = "09098887656",
            //    responseCode = "0",
            //    customerMsg = "Operation successful"

            //};

            //}



            auth = null;

            ObjUtil.WriteErrorLog("XXXXXXXXXXXXXXXXX",
                       "User id:" + userenum.userID + Environment.NewLine + "****" + Environment.NewLine +
                         "Response code: " + userenum.responseCode + " " + Environment.NewLine + "***** " + "Response Msg: " + userenum.customerMsg + " ");



            return userenum;
        }
        //PTB API IMPLEMENTATION
        [System.Web.Http.AcceptVerbs("POST")] // PTB
        [System.Web.Http.Route("api/DraxlaConnect/enumerateCustomers")]

        public UserEnumeration EnumerateCustomers(CustomerAuthentication auth)
        {
            ObjUtil.WriteErrorLog("EnumerateCustomers",
               "Resp endpoint11111111111AAAAAAAAA:" + "mmmmmm" + Environment.NewLine + "****" + Environment.NewLine +
                 "" + Environment.NewLine + "*****" + Environment.NewLine);

            UserEnumeration userenum = new UserEnumeration();
            string apiurl = ConfigurationManager.AppSettings["PTBCustomerEnumeration"].ToString(CultureInfo.InvariantCulture);

            string resp = "";
            Random rx = new Random();
            int rand = rx.Next(10001, 20000);
            string tran_id = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + '-' + rand;
            
            ObjUtil.WriteErrorLog("EnumerateCustomers",
               "Resp endpoint222222222222:" + "mmmmmm" + Environment.NewLine + "****" + Environment.NewLine +
                 "" + Environment.NewLine + "*****" + Environment.NewLine);

            ObjUtil.WriteErrorLog("EnumerateCustomers",
              "Resp endpoint33333333333:" + "mmmmmm" + Environment.NewLine + "****" + Environment.NewLine +
                "token: " + token + " " + Environment.NewLine + "*****" + Environment.NewLine);


            ObjUtil.WriteErrorLog("EnumerateCustomers",
               "Resp endpoint444444:" + token + Environment.NewLine + "****" + Environment.NewLine +
                 "" + Environment.NewLine + "*****" + Environment.NewLine);

            try
           {
                ObjUtil.WriteErrorLog("EnumerateCustomers",
              "Resp endpoint555555 today:" + token + Environment.NewLine + "****" + Environment.NewLine +
                "" + Environment.NewLine + "*****" + Environment.NewLine);



                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
               (se, cert, chain, sslerror) =>
               {
                   return true;
               };

                //HTTP REQUEST
                apiurl = apiurl;
                var client = new RestClient(apiurl);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                // Define the body of the POST request (as JSON or other content type)
                var body = new
                {
                    username = auth.Username,
                    customerId = auth.CustomerId,
                };
                // Serialize the body to JSON (using Newtonsoft.Json or System.Text.Json)
                string jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(body);

                // Add the serialized JSON to the request
                request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

                // Execute the POST request
                IRestResponse response = client.Execute(request);

                // Deserialize the response content
                var hj = response.Content;
                JavaScriptSerializer js = new JavaScriptSerializer();
                var myresponse = js.Deserialize<dynamic>(hj);

                ObjUtil.WriteErrorLog("EnumerateCustomers",
           "Resp endpoint55555555 today2:" + "vvvvvv" + Environment.NewLine + "****" + Environment.NewLine +
             "" + Environment.NewLine + "*****" + Environment.NewLine);

                //var ret = myresponse["data"];
                string respCode = myresponse["responseCode"];
                string respMsg = myresponse["responseMessage"];

                ObjUtil.WriteErrorLog("EnumerateCustomers",
              "Call endpoint: " + apiurl + Environment.NewLine + "****" + Environment.NewLine +
                "" + Environment.NewLine + "*****" + Environment.NewLine);


                ObjUtil.WriteErrorLog("EnumerateCustomers",
                "Resp endpointvvv:" + hj.ToString() + Environment.NewLine + "****" + Environment.NewLine +
                  "" + Environment.NewLine + "*****" + Environment.NewLine);



                if (myresponse["data"] != null)
                {
                    string fullname = myresponse["data"]["customerFirstName"] + " " + myresponse["data"]["customerMiddleName"] + " " + myresponse["data"]["customerLastName"] + " " + myresponse["data"]["customerEmail"];
                    // fullname = fullname.Replace(",", " ");


                    ObjUtil.WriteErrorLog("EnumerateCustomers",
               "Resp endpoint66666 todayno:" + fullname + Environment.NewLine + "****" + Environment.NewLine +
                 "" + Environment.NewLine + "*****" + Environment.NewLine);


                    if (respCode == "00")
                    {
                        respCode = "0";
                    }
                    userenum = new UserEnumeration()
                    {
                        //were the reponses took place
                        responseCode = respCode,
                        customerMsg = respMsg,
                        //customerFirstName = myresponse["data"]["customerFirstName"],
                        // customerMiddleName = myresponse["data"]["customerMiddleName"],
                        //customerLastName = myresponse["data"]["customerLastName"],
                        //customerEmail = myresponse["data"]["customerEmail"],
                        customerFirstName = "Oluwaseun",
                        customerMiddleName ="",
                        customerLastName = "Adeagbo",
                        customerEmail = ""
                        

                    };
                
                }
                else
                {
                    userenum = new UserEnumeration()
                    {
                      
                        
                        customerFirstName = "",
                        customerLastName = "",
                        customerMiddleName = "",
                        customerEmail = "",
                        responseCode = respCode.ToString(),
                        customerMsg = respMsg

                    };
                }

                ObjUtil.WriteErrorLog("EnumerateRetailUser",
               "Resp endpoint77777:" + "" + Environment.NewLine + "****" + Environment.NewLine +
                 "" + Environment.NewLine + "*****" + Environment.NewLine);


            }
            catch (Exception ex)
            {

                ObjUtil.WriteErrorLog("EnumerateRetailUser error",
                   "Error:" + ex.ToString() + Environment.NewLine + "****" + Environment.NewLine +
                     "" + Environment.NewLine + "*****" + Environment.NewLine);

                userenum = new UserEnumeration()
                {

                    //userID = auth.userid,
                    customerFullname = "",
                    customerEmail = "",
                    customerPhoneNo = "",
                    responseCode = "1",
                    customerMsg = "An error occured on enumerating"

                };



            }

            auth = null;

            ObjUtil.WriteErrorLog("XXXXXXXXXXXXXXXXX",
                       "User id:" + userenum.userID + Environment.NewLine + "****" + Environment.NewLine +
                         "Response code: " + userenum.responseCode + " " + Environment.NewLine + "***** " + "Response Msg: " + userenum.customerMsg + " ");



            return userenum;
    }



        [System.Web.Http.AcceptVerbs("POST")] // polaris
        [System.Web.Http.Route("api/DraxlaConnect/enumerateCorporateUser")]
        public UserEnumeration EnumerateCorporateUser(Authentication auth)
        {


            //{"responseCode":"00","responseMessage":"success","data":{"corporateId":"R000001395","username":"1000000581","firstName":"Dennis","middleName":"Lekan","lastName":"Ijeh","registeredEmail":"dennisijeh@gmail.com","registeredPhoneNumber":"2348105143688"}} 


            ObjUtil.WriteErrorLog("enumerateCorporateUser",
               "Resp endpoint11111111111:" + "mmmmmm" + Environment.NewLine + "" + Environment.NewLine +
                 "" + Environment.NewLine + "*" + Environment.NewLine);


            //if (auth.sno == null)
            //{
            //    auth.sno = "0909090909";
            //}

            UserEnumeration userenum = new UserEnumeration();

            try
            {
                string apiurl = ConfigurationManager.AppSettings["polarisCorporateEnumeration"].ToString(CultureInfo.InvariantCulture);

                string resp = "";
                Random rx = new Random();
                int rand = rx.Next(10001, 20000);
                string tran_id = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + '-' + rand;

                //Encryption ect = new Encryption();
                //string UserId = ect.Encrypt(auth.userid);
                //string TranId = ect.Encrypt(tran_id);


                ObjUtil.WriteErrorLog("enumerateCorporateUser",
                   "Resp endpoint222222222222:" + "mmmmmm" + Environment.NewLine + "" + Environment.NewLine +
                     "" + Environment.NewLine + "*" + Environment.NewLine);




                //if (token.Trim() == "")
                //{
                //    token = GetToken();
                //}

                ObjUtil.WriteErrorLog("enumerateCorporateUser",
                   "Resp endpoint33333333333:" + "mmmmmm" + Environment.NewLine + "" + Environment.NewLine +
                     "" + Environment.NewLine + "*" + Environment.NewLine);


                ObjUtil.WriteErrorLog("enumerateCorporateUser",
                   "Resp endpoint444444 Formerly stops here:" + token + Environment.NewLine + "" + Environment.NewLine +
                     "" + Environment.NewLine + "*" + Environment.NewLine);




                //if (token.Trim() == "")
                //{
                //    userenum = new UserEnumeration()
                //    {
                //        userID = "",
                //        sno = "",
                //        customerFirstName = "",
                //        customerLastName = "",
                //        customerMiddleName = "",
                //        customerEmail = "",
                //        customerPhoneNo = "",
                //        responseCode = "-1",
                //        customerMsg = "Token cannot be retrieved at this time. Please retry."

                //    };

                //}
                //else
                //{

                ObjUtil.WriteErrorLog("XXXXXXXXXXXXXXXXXXX",
              "Resp endpoint: " + token + "" + "" + " " + Environment.NewLine + "" + Environment.NewLine +
                "" + Environment.NewLine + "*" + Environment.NewLine);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
               (se, cert, chain, sslerror) =>
               {
                   return true;
               };

                //string corporateId = auth.accesscode;
                //string corpId = corporateId.Replace("_", "");
                apiurl = apiurl + auth.accesscode + "&customerId=" + auth.userid + "&username=" + auth.userid;
                var client = new RestClient(apiurl);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                // request.AddHeader("Authorization", "Bearer " + token);
                var body = @"";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                var hj = response.Content;
                JavaScriptSerializer js = new JavaScriptSerializer();
                var myresponse = js.Deserialize<dynamic>(hj);
                //var ret = myresponse["data"];


                ObjUtil.WriteErrorLog("XXXXXXXXXXXXXXXXXXX",
             "Resp endpoint: " + token + "" + "" + " " + Environment.NewLine + "" + Environment.NewLine +
               "" + Environment.NewLine + "*" + Environment.NewLine);




                ObjUtil.WriteErrorLog("XXXXXXXXXXXXXXXXXXX2222",
             "hj: " + hj.ToString() + "" + "" + " " + Environment.NewLine + "" + Environment.NewLine +
               "" + Environment.NewLine + "*" + Environment.NewLine);


                // int respCode = myresponse["responseCode"];
                //string respMsg = myresponse["responseMessage"];

                string respCode = myresponse["responseCode"];
                string respMsg = myresponse["responseMessage"];

                ObjUtil.WriteErrorLog("enumerateCorporateUser",
              "Call endpoint: " + apiurl + " " + Environment.NewLine + "" + Environment.NewLine +
                "" + Environment.NewLine + "*" + Environment.NewLine);


                ObjUtil.WriteErrorLog("enumerateCorporateUser",
                "Resp endpointvvv:" + hj.ToString() + Environment.NewLine + "" + Environment.NewLine +
                  "" + Environment.NewLine + "*" + Environment.NewLine);



                if (myresponse["data"] != null)
                {
                    string fullname = myresponse["data"]["lastName"] + " " + myresponse["data"]["firstName"] + " " + myresponse["data"]["middleName"];
                    // fullname = fullname.Replace(",", " ");


                    ObjUtil.WriteErrorLog("enumerateCorporateUser",
               "Resp endpoint66666:" + fullname + Environment.NewLine + "" + Environment.NewLine +
                 "" + Environment.NewLine + "*" + Environment.NewLine);



                    userenum = new UserEnumeration()
                    {

                        //userID = auth.userid,
                        // userID = myresponse["userId"],
                        //userID = myresponse["result"]["corporateId"] + "_" + myresponse["data"]["registeredEmail"],

                        userID = myresponse["data"]["username"],
                        sno = auth.sno,
                        customerFullname = fullname,
                        //customerFirstName = myresponse["data"]["customerFirstName"],
                        //customerLastName = myresponse["data"]["customerLastName"],
                        //customerMiddleName = myresponse["data"]["customerLastName"],
                        customerEmail = myresponse["result"]["email"],
                        customerPhoneNo = myresponse["result"]["phoneNumber"],
                        //responseCode = respCode.ToString(),
                        responseCode = "0",
                        customerMsg = respMsg





                    };

                }
                else
                {
                    userenum = new UserEnumeration()
                    {
                        userID = "",
                        sno = "",
                        customerFirstName = "",
                        customerLastName = "",
                        customerMiddleName = "",
                        customerEmail = "",
                        customerPhoneNo = "",
                        responseCode = respCode.ToString(),
                        customerMsg = respMsg

                    };
                }

                ObjUtil.WriteErrorLog("enumerateCorporateUser",
               "Resp endpoint77777:" + "" + Environment.NewLine + "" + Environment.NewLine +
                 "" + Environment.NewLine + "*" + Environment.NewLine);



                //userenum = new UserEnumeration()
                //{
                //    userID = auth.userid,
                //    sno = auth.sno,
                //    customerFirstName = "Feyisara",
                //    customerLastName = "Taiwo",
                //    customerMiddleName = "Kemi",
                //    customerFullname = "Taiwo Feyisara Kemi",
                //    customerEmail = "feyisara@maxut.com",
                //    customerPhoneNo = "09098887656",
                //    responseCode = "0",
                //    customerMsg = "Operation successful"

                //};

                // }
            }
            catch (Exception ex)
            {
                ObjUtil.WriteErrorLog("enumerateCorporateUser",
                 "Error:" + ex.ToString() + " " + Environment.NewLine + "" + Environment.NewLine +
                    "" + Environment.NewLine + "*" + Environment.NewLine);

                userenum = new UserEnumeration()
                {
                    userID = "",
                    sno = "",
                    customerFirstName = "",
                    customerLastName = "",
                    customerMiddleName = "",
                    customerEmail = "",
                    customerPhoneNo = "",
                    responseCode = "-4",
                    customerMsg = "An error occured."

                };

            }

            auth = null;

            return userenum;
        }
        //[System.Web.Http.AcceptVerbs("POST")] // Globus
        //[System.Web.Http.Route("api/DraxlaConnect/enumerateCorporateUser")]
        //public UserEnumeration EnumerateCorporateUser(Authentication auth)
        //{


        //    //{"responseCode":"00","responseMessage":"success","data":{"corporateId":"R000001395","username":"1000000581","firstName":"Dennis","middleName":"Lekan","lastName":"Ijeh","registeredEmail":"dennisijeh@gmail.com","registeredPhoneNumber":"2348105143688"}} 


        //    ObjUtil.WriteErrorLog("enumerateCorporateUser",
        //       "Resp endpoint11111111111:" + "mmmmmm" + Environment.NewLine + "" + Environment.NewLine +
        //         "" + Environment.NewLine + "*" + Environment.NewLine);


        //    //if (auth.sno == null)
        //    //{
        //    //    auth.sno = "0909090909";
        //    //}

        //    UserEnumeration userenum = new UserEnumeration();

        //    try
        //    {
        //        string apiurl = ConfigurationManager.AppSettings["globusCorporateEnumeration"].ToString(CultureInfo.InvariantCulture);

        //        string resp = "";
        //        Random rx = new Random();
        //        int rand = rx.Next(10001, 20000);
        //        string tran_id = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + '-' + rand;

        //        //Encryption ect = new Encryption();
        //        //string UserId = ect.Encrypt(auth.userid);
        //        //string TranId = ect.Encrypt(tran_id);


        //        ObjUtil.WriteErrorLog("enumerateCorporateUser",
        //           "Resp endpoint222222222222:" + "mmmmmm" + Environment.NewLine + "" + Environment.NewLine +
        //             "" + Environment.NewLine + "*" + Environment.NewLine);




        //        //if (token.Trim() == "")
        //        //{
        //        //    token = GetToken();
        //        //}

        //        ObjUtil.WriteErrorLog("enumerateCorporateUser",
        //           "Resp endpoint33333333333:" + "mmmmmm" + Environment.NewLine + "" + Environment.NewLine +
        //             "" + Environment.NewLine + "*" + Environment.NewLine);


        //        ObjUtil.WriteErrorLog("enumerateCorporateUser",
        //           "Resp endpoint444444 Formerly stops here:" + token + Environment.NewLine + "" + Environment.NewLine +
        //             "" + Environment.NewLine + "*" + Environment.NewLine);




        //        //if (token.Trim() == "")
        //        //{
        //        //    userenum = new UserEnumeration()
        //        //    {
        //        //        userID = "",
        //        //        sno = "",
        //        //        customerFirstName = "",
        //        //        customerLastName = "",
        //        //        customerMiddleName = "",
        //        //        customerEmail = "",
        //        //        customerPhoneNo = "",
        //        //        responseCode = "-1",
        //        //        customerMsg = "Token cannot be retrieved at this time. Please retry."

        //        //    };

        //        //}
        //        //else
        //        //{

        //            ObjUtil.WriteErrorLog("XXXXXXXXXXXXXXXXXXX",
        //          "Resp endpoint: " + token + "" + "" + " " + Environment.NewLine + "" + Environment.NewLine +
        //            "" + Environment.NewLine + "*" + Environment.NewLine);

        //            ServicePointManager.Expect100Continue = true;
        //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
        //           (se, cert, chain, sslerror) =>
        //           {
        //               return true;
        //           };

        //        string corporateId = auth.accesscode;
        //        string corpId = corporateId.Replace("_", "");
        //        apiurl = apiurl + corpId;
        //        var client = new RestClient(apiurl);
        //            client.Timeout = -1;
        //            var request = new RestRequest(Method.GET);
        //            // request.AddHeader("Authorization", "Bearer " + token);
        //            var body = @"";
        //            request.AddParameter("text/plain", body, ParameterType.RequestBody);
        //            IRestResponse response = client.Execute(request);

        //            var hj = response.Content;
        //            JavaScriptSerializer js = new JavaScriptSerializer();
        //            var myresponse = js.Deserialize<dynamic>(hj);
        //            //var ret = myresponse["data"];


        //            ObjUtil.WriteErrorLog("XXXXXXXXXXXXXXXXXXX",
        //         "Resp endpoint: " + token + "" + "" + " " + Environment.NewLine + "" + Environment.NewLine +
        //           "" + Environment.NewLine + "*" + Environment.NewLine);




        //            ObjUtil.WriteErrorLog("XXXXXXXXXXXXXXXXXXX2222", 
        //         "hj: " + hj.ToString() + "" + "" + " " + Environment.NewLine + "" + Environment.NewLine +
        //           "" + Environment.NewLine + "*" + Environment.NewLine);


        //            // int respCode = myresponse["responseCode"];
        //            //string respMsg = myresponse["responseMessage"];

        //            string respCode = myresponse["responseCode"];
        //            string respMsg = myresponse["responseMessage"];

        //            ObjUtil.WriteErrorLog("enumerateCorporateUser",
        //          "Call endpoint: " + apiurl + " " + Environment.NewLine + "" + Environment.NewLine +
        //            "" + Environment.NewLine + "*" + Environment.NewLine);


        //            ObjUtil.WriteErrorLog("enumerateCorporateUser",
        //            "Resp endpointvvv:" + hj.ToString() + Environment.NewLine + "" + Environment.NewLine +
        //              "" + Environment.NewLine + "*" + Environment.NewLine);



        //            if (myresponse["result"] != null)
        //            {
        //                string fullname = myresponse["result"]["lastName"] + " " + myresponse["result"]["firstName"] + " " + myresponse["result"]["middleName"];
        //                // fullname = fullname.Replace(",", " ");


        //                ObjUtil.WriteErrorLog("enumerateCorporateUser",
        //           "Resp endpoint66666:" + fullname + Environment.NewLine + "" + Environment.NewLine +
        //             "" + Environment.NewLine + "*" + Environment.NewLine);



        //                userenum = new UserEnumeration()
        //                {

        //                    //userID = auth.userid,
        //                    // userID = myresponse["userId"],
        //                    //userID = myresponse["result"]["corporateId"] + "_" + myresponse["data"]["registeredEmail"],

        //                    userID = myresponse["result"]["userId"],
        //                    sno = auth.sno,
        //                    customerFullname = fullname,
        //                    //customerFirstName = myresponse["data"]["customerFirstName"],
        //                    //customerLastName = myresponse["data"]["customerLastName"],
        //                    //customerMiddleName = myresponse["data"]["customerLastName"],
        //                    customerEmail = myresponse["result"]["registeredEmailAddress"],
        //                    customerPhoneNo = myresponse["result"]["registeredPhoneNumber"],
        //                    //responseCode = respCode.ToString(),
        //                    responseCode = "0",
        //                    customerMsg = respMsg





        //                };

        //            }
        //            else
        //            {
        //                userenum = new UserEnumeration()
        //                {
        //                    userID = "",
        //                    sno = "",
        //                    customerFirstName = "",
        //                    customerLastName = "",
        //                    customerMiddleName = "",
        //                    customerEmail = "",
        //                    customerPhoneNo = "",
        //                    responseCode = respCode.ToString(),
        //                    customerMsg = respMsg

        //                };
        //            }

        //            ObjUtil.WriteErrorLog("enumerateCorporateUser",
        //           "Resp endpoint77777:" + "" + Environment.NewLine + "" + Environment.NewLine +
        //             "" + Environment.NewLine + "*" + Environment.NewLine);



        //            //userenum = new UserEnumeration()
        //            //{
        //            //    userID = auth.userid,
        //            //    sno = auth.sno,
        //            //    customerFirstName = "Feyisara",
        //            //    customerLastName = "Taiwo",
        //            //    customerMiddleName = "Kemi",
        //            //    customerFullname = "Taiwo Feyisara Kemi",
        //            //    customerEmail = "feyisara@maxut.com",
        //            //    customerPhoneNo = "09098887656",
        //            //    responseCode = "0",
        //            //    customerMsg = "Operation successful"

        //            //};

        //       // }
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjUtil.WriteErrorLog("enumerateCorporateUser",
        //         "Error:" + ex.ToString() + " " + Environment.NewLine + "" + Environment.NewLine +
        //            "" + Environment.NewLine + "*" + Environment.NewLine);

        //        userenum = new UserEnumeration()
        //        {
        //            userID = "",
        //            sno = "",
        //            customerFirstName = "",
        //            customerLastName = "",
        //            customerMiddleName = "",
        //            customerEmail = "",
        //            customerPhoneNo = "",
        //            responseCode = "-4",
        //            customerMsg = "An error occured."

        //        };

        //    }

        //    auth = null;

        //    return userenum;
        //}




        //[System.Web.Http.AcceptVerbs("POST")]
        //[System.Web.Http.Route("api/DraxlaConnect/enumerateCorporateUser")]
        //public UserEnumeration EnumerateCorporateUser(Authentication auth)
        //{


        //    //{"responseCode":"00","responseMessage":"success","data":{"corporateId":"R000001395","username":"1000000581","firstName":"Dennis","middleName":"Lekan","lastName":"Ijeh","registeredEmail":"dennisijeh@gmail.com","registeredPhoneNumber":"2348105143688"}} 




        //    UserEnumeration userenum = new UserEnumeration();

        //    try
        //    {

        //       JavaScriptSerializer js = new JavaScriptSerializer();
        //        var hj = "{'responseCode':'00','responseMessage':'success','data':{'corporateId':'R000001395','username':'1000000581','firstName':'Dennis','middleName':'Lekan','lastName':'Ijeh','registeredEmail':'dennisijeh@gmail.com','registeredPhoneNumber':'2348105143688'}}";
        //             var myresponse = js.Deserialize<dynamic>(hj);




        //            string respCode = myresponse["responseCode"];
        //            string respMsg = myresponse["responseMessage"];

        //            if (myresponse["data"] != null)
        //            {
        //                string fullname = myresponse["data"]["lastName"] + " " + myresponse["data"]["firstName"] + " " + myresponse["data"]["middleName"];
        //                // fullname = fullname.Replace(",", " ");



        //                userenum = new UserEnumeration()
        //                {

        //                    //userID = auth.userid,
        //                    // userID = myresponse["userId"],
        //                    userID = myresponse["data"]["corporateId"] + "_" + myresponse["data"]["registeredEmail"],
        //                    sno = auth.sno,
        //                    customerFullname = fullname,
        //                    //customerFirstName = myresponse["data"]["customerFirstName"],
        //                    //customerLastName = myresponse["data"]["customerLastName"],
        //                    //customerMiddleName = myresponse["data"]["customerLastName"],
        //                    customerEmail = myresponse["data"]["registeredEmail"],
        //                    customerPhoneNo = myresponse["data"]["registeredPhoneNumber"],
        //                    responseCode = respCode.ToString(),
        //                    customerMsg = respMsg

        //                };

        //            }
        //            else
        //            {
        //                userenum = new UserEnumeration()
        //                {
        //                    userID = "",
        //                    sno = "",
        //                    customerFirstName = "",
        //                    customerLastName = "",
        //                    customerMiddleName = "",
        //                    customerEmail = "",
        //                    customerPhoneNo = "",
        //                    responseCode = respCode.ToString(),
        //                    customerMsg = respMsg

        //                };
        //            }



        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    auth = null;

        //    return userenum;
        //}



        //  [AcceptVerbs("POST")]
        public SendSMSResponse SendSMS(Authcode model)
        {
            int respCode = 1; string respMsg = "";

            string ApiResCode = "";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                      | SecurityProtocolType.Tls
                      | SecurityProtocolType.Tls11
                      | SecurityProtocolType.Tls12;


            if (token.Trim() == "")
            {
                token = GetToken();
            }


            // string api = "https://optiweb.optimusbank.com:8025/api-gateway/tfa/send-sms-auth-code";
            string api = ConfigurationManager.AppSettings["SendSMSOTP"].ToString(CultureInfo.InvariantCulture);

            var client = new RestClient(api);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            // request.AddHeader("Authorization", "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiZDI1ZTE5MjUtYzA3NC00NWYyLTkzYmQtMDVjODRjZDcyODU2IiwiY29ubmVjdGVkRnJvbSI6IjE3Mi4xNi4zMC4yMCIsInRva2VuRGF0ZSI6IjIwMjMtMTAtMjYgMTA6NTk6MTMuODE3IiwiY2hhbm5lbCI6Ik1heHV0IDJGQSBDbGllbnQiLCJqdGkiOiI2NzExM2ZkMy00YjQwLTQ5ZjEtOGE4Yy0xNjAzNzkwZDcxYzQiLCJleHAiOjE2OTgzMTQ5NTMsImlzcyI6Imh0dHA6Ly8xOTIuMTY4LjEwMi44My9hcGlpZG0vIiwiYXVkIjoiaHR0cDovLzE5Mi4xNjguMTAyLjgzL2FwaWlkbS8ifQ.beZJYN12muMaNB7P_Q2I8OPkAHijx8gPmdm7IIWUFqk");
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiZDI1ZTE5MjUtYzA3NC00NWYyLTkzYmQtMDVjODRjZDcyODU2IiwiY29ubmVjdGVkRnJvbSI6IjE3Mi4xNi4zMC4yMCIsInRva2VuRGF0ZSI6IjIwMjMtMTAtMjYgMTA6NTk6MTMuODE3IiwiY2hhbm5lbCI6Ik1heHV0IDJGQSBDbGllbnQiLCJqdGkiOiI2NzExM2ZkMy00YjQwLTQ5ZjEtOGE4Yy0xNjAzNzkwZDcxYzQiLCJleHAiOjE2OTgzMTQ5NTMsImlzcyI6Imh0dHA6Ly8xOTIuMTY4LjEwMi44My9hcGlpZG0vIiwiYXVkIjoiaHR0cDovLzE5Mi4xNjguMTAyLjgzL2FwaWlkbS8ifQ.beZJYN12muMaNB7P_Q2I8OPkAHijx8gPmdm7IIWUFqk");


            JObject objectbody = new JObject();
            objectbody.Add("phoneNumber", model.telephone);
            objectbody.Add("messageBody", model.message_body);

            request.AddParameter("Application/json", objectbody, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);


            ObjUtil.WriteErrorLog("SmsCall",
           "url: " + api + Environment.NewLine + "" + Environment.NewLine +
         "payload: " + objectbody + Environment.NewLine + "*" + Environment.NewLine + "");




            var hj = response.Content;
            JavaScriptSerializer js = new JavaScriptSerializer();
            var myresponse = js.Deserialize<dynamic>(hj);

            //ApiResCode = myresponse["responseCode"];
            //respMsg = myresponse["responseMessage"];

            ObjUtil.WriteErrorLog("SmsResponse",
            "resp code: " + ApiResCode + Environment.NewLine + "" + Environment.NewLine +
             "resp msg: " + respMsg + Environment.NewLine + "*" + Environment.NewLine + "");

            SendSMSResponse SmsResponse = new SendSMSResponse();

            //SendSMSResponse responseData = JsonConvert.DeserializeObject<SendSMSResponse>(response.Content);

            if (ApiResCode == "00")
            {
                SmsResponse.responseMessage = respMsg;
            }
            else
            {
                SmsResponse.responseMessage = respMsg;
            }




            return SmsResponse;
        }


        //sendsms
        public sendemailResponse SendEmail(Authcode model)
        {
            int respCode = 1; string respMsg = "";


            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                       | SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12;

            if (token.Trim() == "")
            {
                token = GetToken();
            }


            string ApiResCode = "";
            string api = "https://optiweb.optimusbank.com:8025/api-gateway/tfa/send-email-auth-code";

            sendemailResponse sendemailResponse = new sendemailResponse();

            var client = new RestClient(api);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            // request.AddHeader("Authorization", "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiZDI1ZTE5MjUtYzA3NC00NWYyLTkzYmQtMDVjODRjZDcyODU2IiwiY29ubmVjdGVkRnJvbSI6IjE3Mi4xNi4zMC4yMCIsInRva2VuRGF0ZSI6IjIwMjMtMTAtMjYgMTA6NTQ6MzMuNzMyIiwiY2hhbm5lbCI6Ik1heHV0IDJGQSBDbGllbnQiLCJqdGkiOiJjZGRiNWQ4OS1kYmY5LTQxZjEtYjBhNi1mNmE2NTJjNDhiYjciLCJleHAiOjE2OTgzMTQ2NzMsImlzcyI6Imh0dHA6Ly8xOTIuMTY4LjEwMi44My9hcGlpZG0vIiwiYXVkIjoiaHR0cDovLzE5Mi4xNjguMTAyLjgzL2FwaWlkbS8ifQ.qr5Tp9JS50Hc9IEBAyACwHm5Ol_JItVjL6GACt3q3Vg");
            request.AddHeader("Authorization", "Bearer " + token);

            JObject objectbody = new JObject();
            objectbody.Add("emailAddress", model.email_address);
            objectbody.Add("messageBody", model.message_body);
            objectbody.Add("messageSubject", model.message_subject);


            request.AddParameter("Application/json", objectbody, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);


            ObjUtil.WriteErrorLog("EmailCall",
      "url: " + api + Environment.NewLine + "" + Environment.NewLine +
    "payload: " + objectbody + Environment.NewLine + "*" + Environment.NewLine + "");



            var hj = response.Content;
            JavaScriptSerializer js = new JavaScriptSerializer();
            var myresponse = js.Deserialize<dynamic>(hj);

            //ApiResCode = myresponse["responseCode"];
           // respMsg = myresponse["responseMessage"];

            ObjUtil.WriteErrorLog("EmailResponse",
        "resp code: " + ApiResCode + Environment.NewLine + "" + Environment.NewLine +
         "resp msg: " + respMsg + Environment.NewLine + "*" + Environment.NewLine + "");

            if (ApiResCode == "00")
            {

                sendemailResponse.responseMessage = respMsg;
                // xml = "<?xml version='1.0' encoding='UTF-8'?><DP4Apps><retCode>" + respCode + "</retCode><retMsg>" + respMsg + "</retMsg></DP4Apps>";

            }
            else
            {
                sendemailResponse.responseMessage = respMsg;

            }



            return sendemailResponse;


        }

        public string SendSMSAfterEnumeration(string userid, string corporateid)
        {
            string respCode = "1"; string respMsg = "failed";

            string resp = "1|failed";

           ObjUtil.WriteErrorLog("SmsAfter",
           "userid: " + userid + Environment.NewLine + "" + Environment.NewLine +
           "corporateid : " + corporateid + Environment.NewLine + "*" + Environment.NewLine + "" + "");



            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                      | SecurityProtocolType.Tls
                      | SecurityProtocolType.Tls11
                      | SecurityProtocolType.Tls12;


            if (token == "")
            {
                token = GetToken();
            }

            if (token == "")
            {
                respCode = "-1";
                respMsg = "Token cannot be retrieved at this time. Please retry.";
            
            }
            else
            {
                // string api = "https://optiweb.optimusbank.com:8025/api-gateway/tfa/send-sms-auth-code";
                string apiurl = "";
                string payload = "";



                if (corporateid == "")
                {
                    apiurl = ConfigurationManager.AppSettings["SendSMSAfterEnumeration"].ToString(CultureInfo.InvariantCulture);
                    payload = "{\r\n    \"userId\" : \"" + userid + "\"\r\n}";
                }
                else
                {
                    apiurl = ConfigurationManager.AppSettings["SendSMSAfterCorporateEnumeration"].ToString(CultureInfo.InvariantCulture);
                    payload = "{\r\n    \"corporateId\" : \"" + corporateid + "\",\r\n    \"username\" : \"" + userid + "\"\r\n}";

                }

                var client = new RestClient(apiurl);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                // request.AddHeader("Authorization", "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiZDI1ZTE5MjUtYzA3NC00NWYyLTkzYmQtMDVjODRjZDcyODU2IiwiY29ubmVjdGVkRnJvbSI6IjE3Mi4xNi4zMC4yMCIsInRva2VuRGF0ZSI6IjIwMjMtMTAtMjYgMTA6NTk6MTMuODE3IiwiY2hhbm5lbCI6Ik1heHV0IDJGQSBDbGllbnQiLCJqdGkiOiI2NzExM2ZkMy00YjQwLTQ5ZjEtOGE4Yy0xNjAzNzkwZDcxYzQiLCJleHAiOjE2OTgzMTQ5NTMsImlzcyI6Imh0dHA6Ly8xOTIuMTY4LjEwMi44My9hcGlpZG0vIiwiYXVkIjoiaHR0cDovLzE5Mi4xNjguMTAyLjgzL2FwaWlkbS8ifQ.beZJYN12muMaNB7P_Q2I8OPkAHijx8gPmdm7IIWUFqk");
                request.AddHeader("Authorization", "Bearer " + token);


                Encryption en = new Encryption();
                string enc = en.EncryptPayload(payload);
                //jObjectbody = null;
                //JObject jObjectbody2 = new JObject();
                //jObjectbody2.Add("requestStr", enc);

                string payload2 = "{\r\n    \"requestStr\" : \"" + enc + "\"\r\n}";
                //string payload = "{\r\n    \"requestStr\" : \"" + enc + "\"\r\n}";


                ObjUtil.WriteErrorLog("IB 111111111111111-tiiii",
                "apiurl: " + apiurl + Environment.NewLine + "****" + Environment.NewLine +
                "body---- : " + payload + Environment.NewLine + "*****" + Environment.NewLine + " payload2: " + payload2);


                request.AddParameter("Application/json", payload2, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);


                ObjUtil.WriteErrorLog("SmsCall",
               "url: " + apiurl + Environment.NewLine + "" + Environment.NewLine +
             "payloadEnc: " + payload2 + Environment.NewLine + "*" + Environment.NewLine + " payload: " + payload);




                var hj = response.Content;
                JavaScriptSerializer js = new JavaScriptSerializer();
                var myresponse = js.Deserialize<dynamic>(hj);

                
                ObjUtil.WriteErrorLog("IB validation 222222 --------------222222222222",
                                "Response -----222: " + hj.ToString() + Environment.NewLine + "****" + Environment.NewLine +
                                "ResponseStatus -----222: " + response.ResponseStatus + " | sttus: " + response.StatusCode + " | descr: " + response.StatusDescription + " " + Environment.NewLine + "*****" + Environment.NewLine + "");


                
                if (myresponse != null)
                {
                    respCode = myresponse["responseCode"];
                    respMsg = myresponse["responseMessage"];

                    //if (myresponse["data"] != null)
                    if (respCode != "00")
                    {
                        respCode = myresponse["responseCode"];
                        respMsg = myresponse["responseMessage"];
                    }
                    else
                    {
                        respCode = "0";
                        respMsg = myresponse["data"];

                
                        ObjUtil.WriteErrorLog("EnumerateRetailUser successful 101",
                                          "UserId101: " + myresponse["data"] + Environment.NewLine + "****" + Environment.NewLine +
                                          "fullname: " + "" + Environment.NewLine + "*****" + "sno: " + "");

                        //  ObjUtil.WriteErrorLog("EnumerateRetailUser",
                        //"Resp code: " + respCode + Environment.NewLine + "****" + Environment.NewLine +
                        //"Resp code: " + respMsg + Environment.NewLine + "*****" + Environment.NewLine + " User ID: " + auth.userid);
                    }

                }
                else
                {
                    ObjUtil.WriteErrorLog("EnumerateRetailUser no data response ",
                   "Resp code: " + respCode + Environment.NewLine + "****" + Environment.NewLine +
                   "Resp code: " + respMsg + Environment.NewLine + "*****" + "No data is returned for this user" + " User ID: " + "");

                    respCode = "1";
                    respMsg = "No response";

                   


                }
            }



            resp = respCode + "|" + respMsg;



            return resp;
        }


        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.Route("api/DraxlaConnect/validateOTPAfterEnumeration/{otp}/{referenceid}")]
        public ResponseObj ValidateOTPAfterEnumeration(string otp, string referenceid)
        {
            string respCode = "1"; string respMsg = "failed";

            respobj = new ResponseObj();

            ObjUtil.WriteErrorLog("SmsAfter",
            "otp: " + otp + Environment.NewLine + "" + Environment.NewLine +
            "corporateid : " + referenceid + Environment.NewLine + "*" + Environment.NewLine + "" + "");



            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                      | SecurityProtocolType.Tls
                      | SecurityProtocolType.Tls11
                      | SecurityProtocolType.Tls12;


            if (token == "")
            {
                token = GetToken();
            }

            if (token == "")
            {
                respobj.RespCode = "-1";
                respobj.RespMsg = "Token cannot be retrieved at this time. Please retry.";

            }
            else
            {
                // string api = "https://optiweb.optimusbank.com:8025/api-gateway/tfa/send-sms-auth-code";
                string apiurl = "";
                string payload = "";



               
                    apiurl = ConfigurationManager.AppSettings["ValidateSMSOTPAfterEnumeration"].ToString(CultureInfo.InvariantCulture);
                    payload = "{\r\n    \"token\" : \"" + otp + "\",\r\n    \"referenceId\" : \"" + referenceid + "\"\r\n}";

               

                var client = new RestClient(apiurl);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                // request.AddHeader("Authorization", "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiZDI1ZTE5MjUtYzA3NC00NWYyLTkzYmQtMDVjODRjZDcyODU2IiwiY29ubmVjdGVkRnJvbSI6IjE3Mi4xNi4zMC4yMCIsInRva2VuRGF0ZSI6IjIwMjMtMTAtMjYgMTA6NTk6MTMuODE3IiwiY2hhbm5lbCI6Ik1heHV0IDJGQSBDbGllbnQiLCJqdGkiOiI2NzExM2ZkMy00YjQwLTQ5ZjEtOGE4Yy0xNjAzNzkwZDcxYzQiLCJleHAiOjE2OTgzMTQ5NTMsImlzcyI6Imh0dHA6Ly8xOTIuMTY4LjEwMi44My9hcGlpZG0vIiwiYXVkIjoiaHR0cDovLzE5Mi4xNjguMTAyLjgzL2FwaWlkbS8ifQ.beZJYN12muMaNB7P_Q2I8OPkAHijx8gPmdm7IIWUFqk");
                request.AddHeader("Authorization", "Bearer " + token);


                Encryption en = new Encryption();
                string enc = en.EncryptPayload(payload);
                //jObjectbody = null;
                //JObject jObjectbody2 = new JObject();
                //jObjectbody2.Add("requestStr", enc);

                string payload2 = "{\r\n    \"requestStr\" : \"" + enc + "\"\r\n}";
                //string payload = "{\r\n    \"requestStr\" : \"" + enc + "\"\r\n}";


                ObjUtil.WriteErrorLog("smsotpv 111111111111111-tiiii",
                "apiurl: " + apiurl + Environment.NewLine + "****" + Environment.NewLine +
                "body---- : " + payload + Environment.NewLine + "*****" + Environment.NewLine + " payload2: " + payload2);


                request.AddParameter("Application/json", payload2, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);


                ObjUtil.WriteErrorLog("SmsCall",
               "url: " + apiurl + Environment.NewLine + "" + Environment.NewLine +
             "payloadEnc: " + payload2 + Environment.NewLine + "*" + Environment.NewLine + " payload: " + payload);




                var hj = response.Content;
                JavaScriptSerializer js = new JavaScriptSerializer();
                var myresponse = js.Deserialize<dynamic>(hj);


                ObjUtil.WriteErrorLog("IB validation 222222 --------------222222222222",
                                "Response -----222: " + hj.ToString() + Environment.NewLine + "****" + Environment.NewLine +
                                "ResponseStatus -----222: " + response.ResponseStatus + " | sttus: " + response.StatusCode + " | descr: " + response.StatusDescription + " " + Environment.NewLine + "*****" + Environment.NewLine + "");



                if (myresponse != null)
                {
                    respobj.RespCode = myresponse["responseCode"];
                    if(respobj.RespCode == "00")
                    {
                        respobj.RespCode = "0";
                    }
                    respobj.RespMsg = myresponse["responseMessage"];

                }
                else
                {
                    ObjUtil.WriteErrorLog("EnumerateRetailUser no data response ",
                   "Resp code: " + respCode + Environment.NewLine + "****" + Environment.NewLine +
                   "Resp code: " + respMsg + Environment.NewLine + "*****" + "No data is returned for this user" + " User ID: " + "");

                    respobj.RespCode = "1";
                    respobj.RespMsg = "No response";




                }
            }



            return respobj;
        }


        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.Route("api/DraxlaConnect/AfterHardwareTokenAssignment/{isAssigned}/{referenceid}")]
        public ResponseObj AfterHardwareTokenAssignment(Boolean isAssigned, string referenceid)
        {
            string respCode = "1"; string respMsg = "failed";
            string otherDetauil = "";

            if (isAssigned)
            {
                otherDetauil = "Token assigned successfully";
            }

            respobj = new ResponseObj();

            ObjUtil.WriteErrorLog("After....",
            "isAssigned: " + isAssigned + Environment.NewLine + "" + Environment.NewLine +
            "corporateid : " + referenceid + Environment.NewLine + "*" + Environment.NewLine + "" + "");



            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                      | SecurityProtocolType.Tls
                      | SecurityProtocolType.Tls11
                      | SecurityProtocolType.Tls12;


            if (token == "")
            {
                token = GetToken();
            }

            if (token == "")
            {
                respobj.RespCode = "-1";
                respobj.RespMsg = "Token cannot be retrieved at this time. Please retry.";

            }
            else
            {
                // string api = "https://optiweb.optimusbank.com:8025/api-gateway/tfa/send-sms-auth-code";
                string apiurl = "";
                string payload = "";




                apiurl = ConfigurationManager.AppSettings["AfterHardwareTokenAssignment"].ToString(CultureInfo.InvariantCulture);
                payload = "{\r\n    \"IsAssigned\" : \"" + isAssigned + "\",\r\n    \"referenceId\" : \"" + referenceid + "\"\r\n}";



                var client = new RestClient(apiurl);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                // request.AddHeader("Authorization", "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiZDI1ZTE5MjUtYzA3NC00NWYyLTkzYmQtMDVjODRjZDcyODU2IiwiY29ubmVjdGVkRnJvbSI6IjE3Mi4xNi4zMC4yMCIsInRva2VuRGF0ZSI6IjIwMjMtMTAtMjYgMTA6NTk6MTMuODE3IiwiY2hhbm5lbCI6Ik1heHV0IDJGQSBDbGllbnQiLCJqdGkiOiI2NzExM2ZkMy00YjQwLTQ5ZjEtOGE4Yy0xNjAzNzkwZDcxYzQiLCJleHAiOjE2OTgzMTQ5NTMsImlzcyI6Imh0dHA6Ly8xOTIuMTY4LjEwMi44My9hcGlpZG0vIiwiYXVkIjoiaHR0cDovLzE5Mi4xNjguMTAyLjgzL2FwaWlkbS8ifQ.beZJYN12muMaNB7P_Q2I8OPkAHijx8gPmdm7IIWUFqk");
                request.AddHeader("Authorization", "Bearer " + token);


                Encryption en = new Encryption();
                string enc = en.EncryptPayload(payload);
                //jObjectbody = null;
                //JObject jObjectbody2 = new JObject();
                //jObjectbody2.Add("requestStr", enc);

                string payload2 = "{\r\n    \"requestStr\" : \"" + enc + "\"\r\n}";
                //string payload = "{\r\n    \"requestStr\" : \"" + enc + "\"\r\n}";


                ObjUtil.WriteErrorLog("smsotpv 111111111111111-tiiii",
                "apiurl: " + apiurl + Environment.NewLine + "****" + Environment.NewLine +
                "body---- : " + payload + Environment.NewLine + "*****" + Environment.NewLine + " payload2: " + payload2);


                request.AddParameter("Application/json", payload2, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);


                ObjUtil.WriteErrorLog("SmsCall",
               "url: " + apiurl + Environment.NewLine + "" + Environment.NewLine +
             "payloadEnc: " + payload2 + Environment.NewLine + "*" + Environment.NewLine + " payload: " + payload);




                var hj = response.Content;
                JavaScriptSerializer js = new JavaScriptSerializer();
                var myresponse = js.Deserialize<dynamic>(hj);


                ObjUtil.WriteErrorLog("IB validation 222222 --------------222222222222",
                                "Response -----222: " + hj.ToString() + Environment.NewLine + "****" + Environment.NewLine +
                                "ResponseStatus -----222: " + response.ResponseStatus + " | sttus: " + response.StatusCode + " | descr: " + response.StatusDescription + " " + Environment.NewLine + "*****" + Environment.NewLine + "");



                if (myresponse != null)
                {
                    respobj.RespCode = myresponse["responseCode"];
                    respobj.RespMsg = myresponse["responseMessage"];

                }
                else
                {
                    ObjUtil.WriteErrorLog("EnumerateRetailUser no data response ",
                   "Resp code: " + respCode + Environment.NewLine + "****" + Environment.NewLine +
                   "Resp code: " + respMsg + Environment.NewLine + "*****" + "No data is returned for this user" + " User ID: " + "");

                    respobj.RespCode = "1";
                    respobj.RespMsg = "No response";




                }
            }

            return respobj;
        }



        //[System.Web.Http.AcceptVerbs("POST")]
        //[System.Web.Http.Route("api/DraxlaConnect/StaffADValidation")]
        //public StaffDetails StaffADValidation(StaffLogin auth)
        //{

        //    ObjUtil.WriteErrorLog("SV 111111AAAAAAAAA 2222222",
        //          "User ID: " + "" + Environment.NewLine + "****" + Environment.NewLine +
        //           "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");


        //    string apiurl = ConfigurationManager.AppSettings["StaffADValidation"].ToString(CultureInfo.InvariantCulture);
        //    StaffDetails staffdetail = new StaffDetails();
        //    int respCode = -1;
        //    string respMsg = "";

        //    ObjUtil.WriteErrorLog("Staffadvalidation 1",
        //              "User ID: " + auth.user_id + Environment.NewLine + "****" + Environment.NewLine +
        //               "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");


        //    string resp = "";
        //    Random rx = new Random();
        //    int rand = rx.Next(10001, 20000);
        //    string tran_id = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + '-' + rand;

        //    Encryption ect = new Encryption();
        //    string userid = ect.Encrypt(auth.user_id);
        //    string Password = ect.Encrypt(auth.password);
        //    string TranId = ect.Encrypt(tran_id);

        //    string token = GetToken();

        //    try
        //    {
        //        ObjUtil.WriteErrorLog("Staffadvalidation 2",
        //           "Token: " + token + Environment.NewLine + "****" + Environment.NewLine +
        //            "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");

        //        if (token.Trim() == "")
        //        {
        //            ObjUtil.WriteErrorLog("Staffadvalidation 3",
        //        "Token: " + token + Environment.NewLine + "****" + Environment.NewLine +
        //         "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");

        //            token = GetToken();

        //            ObjUtil.WriteErrorLog("Staffadvalidation 4",
        //        "Token: " + token + Environment.NewLine + "****" + Environment.NewLine +
        //         "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");

        //        }

        //        if (token.Trim() == "")
        //        {
        //            respCode = -1;
        //            respMsg = "Token cannot be retrieved at this time. Please retry.";

        //        }
        //        else
        //        {

        //            var client = new RestClient(apiurl);
        //            client.Timeout = -1;
        //            var request = new RestRequest(Method.POST);
        //            request.AddHeader("Authorization", token);
        //            request.AddHeader("Content-Type", "application/json");

        //            var body = @"{
        //        " + "\n" +
        //             @"  ""userName"": """ + userid + "" + "\"" + ",\r\n\n " +
        //             @"  ""password"": """ + Password + "" + "\"" + ",\r\n\n " +
        //             @"  ""requestId"": """ + TranId + "" + "\"" + ",\r\n\n " +
        //             @"  ""channel"": ""TP-ETOKEN"" " + "\n" +
        //             @"}";


        //            request.AddParameter("application/json", body, ParameterType.RequestBody);
        //            IRestResponse response = client.Execute(request);
        //            Console.WriteLine(response.Content);

        //            string jsonContent = JsonConvert.SerializeObject(body);


        //            ObjUtil.WriteErrorLog("Staffadvalidation 4b",
        //             "Response: " + response.StatusCode + ", call: " + jsonContent + " " +
        //             Environment.NewLine + "****" + Environment.NewLine +
        //             "Response Msg: " + response.StatusDescription + "| url: " + apiurl + " " + Environment.NewLine + "*****" + Environment.NewLine + "User ID: " + auth.user_id);

        //            try
        //            {
        //                ObjUtil.WriteErrorLog("Staffadvalidation 4c",
        //                 "message: " + response.Content + Environment.NewLine + "****" + Environment.NewLine +
        //                  "Response Msg: " + response.StatusDescription + "|" + Environment.NewLine + "*****" + Environment.NewLine + "User ID: " + auth.user_id);


        //            }
        //            catch (Exception ex)
        //            {
        //                ObjUtil.WriteErrorLog("Staffadvalidation 4c2",
        //                "message: " + ex.ToString() + Environment.NewLine + "****" + Environment.NewLine +
        //                 "Response Msg: " + response.StatusDescription + "|" + Environment.NewLine + "*****" + Environment.NewLine + "User ID: " + auth.user_id);

        //            }


        //            var hj = response.Content;
        //            JavaScriptSerializer js = new JavaScriptSerializer();
        //            var myresponse = js.Deserialize<dynamic>(hj);
        //            var ret = myresponse["user"]; //data


        //            ObjUtil.WriteErrorLog("Staffvalidation",
        //              "Response: " + hj.ToString() + Environment.NewLine + "****" + Environment.NewLine +
        //              "Response Msg: " + myresponse["responseCode"] + "|00000" + myresponse["responseDescription"] + Environment.NewLine + "*****" + Environment.NewLine + "User ID: " + auth.user_id);


        //            if (myresponse != null)
        //            {
        //                respCode = myresponse["responseCode"];
        //                respMsg = myresponse["responseDescription"];


        //                //ObjUtil.WriteErrorLog("Staffvalidation",
        //                //  "Response: " + hj.ToString() + Environment.NewLine + "****" + Environment.NewLine +
        //                //  "Response Msg: " + myresponse["responseCode"] + "|" + myresponse["responseDescription"] + Environment.NewLine + "*****" + Environment.NewLine + "User ID: " + auth.user_id);


        //                //respCode = myresponse["ResponseCode"];
        //                //respMsg = myresponse["ResponseDescription"];

        //                if (myresponse["user"] != null)
        //                {
        //                    staffdetail = new StaffDetails()
        //                    {
        //                        role = myresponse["user"]["jobTitle"],
        //                        branch = myresponse["user"]["branchCode"],
        //                        staff_name = myresponse["user"]["userPrincipalName"],
        //                        fullname = "",
        //                        usertype = 1,
        //                        response_code = respCode,
        //                        response_description = respMsg,
        //                    };

        //                }
        //                else
        //                {
        //                    staffdetail = new StaffDetails()
        //                    {
        //                        role = "",
        //                        branch = "",
        //                        staff_name = "",
        //                        response_code = respCode,
        //                        response_description = respMsg

        //                    };
        //                }
        //            }
        //            else
        //            {
        //                staffdetail = new StaffDetails()
        //                {
        //                    role = "",
        //                    branch = "",
        //                    staff_name = "",
        //                    response_code = 1,
        //                    response_description = "No response is returned from AD"
        //                    //response_description = response.StatusDescription
        //                };
        //            }



        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ObjUtil.WriteErrorLog("Staffvalidation error",
        //                   ex.Message + Environment.NewLine + "************" + Environment.NewLine +
        //                   ex.StackTrace + Environment.NewLine + "***********" + Environment.NewLine + ex.InnerException);

        //        staffdetail = new StaffDetails()
        //        {
        //            role = "",
        //            branch = "",
        //            staff_name = "",
        //            response_code = 4,
        //            response_description = "An error occured on the server"
        //            //response_description = response.StatusDescription
        //        };
        //    }




        //    return staffdetail;
        //}


        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.Route("api/DraxlaConnect/StaffADValidation")]
        public StaffDetails StaffADValidation(StaffLogin auth)
        {

            ObjUtil.WriteErrorLog("SV 111111AAAAAAAAA 2222222",
                  "username: " + "" + Environment.NewLine + "****" + Environment.NewLine +
                   "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");


            string apiurl = ConfigurationManager.AppSettings["StaffADValidation"].ToString(CultureInfo.InvariantCulture);
            StaffDetails staffdetail = new StaffDetails();
            int respCode = -1;
            string respMsg = "";
            string branch_code = "0";

            ObjUtil.WriteErrorLog("Staffadvalidation 1",
                      "User ID: " + auth.user_id + Environment.NewLine + "****" + Environment.NewLine +
                       "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");


            string resp = "";
            Random rx = new Random();
            int rand = rx.Next(10001, 20000);
            string tran_id = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + '-' + rand;

           // Encryption ect = new Encryption();
            string username = auth.user_id;
            string password = auth.password;
            //string TranId = ect.Encrypt(tran_id);

            //string token = GetToken();

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
               (se, cert, chain, sslerror) =>
               {
                   return true;
               };

                {

                    var client = new RestClient(apiurl);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                   // request.AddHeader("Authorization", token);
                    request.AddHeader("Content-Type", "application/json");

                    var body = @"{
                " + "\n" +
                     @"  ""username"": """ + username + "" + "\"" + ",\r\n\n " +
                     @"  ""password"": """ + password + "" + "\"" + ",\r\n\n " +
                     @"}";


                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);

                    string jsonContent = JsonConvert.SerializeObject(body);


                    ObjUtil.WriteErrorLog("Staffadvalidation 4b",
                     "Response: " + response.StatusCode + ", call: " + jsonContent + " " +
                     Environment.NewLine + "****" + Environment.NewLine +
                     "Response Msg: " + response.StatusDescription + "| url: " + apiurl + " " + Environment.NewLine + "*****" + Environment.NewLine + "User ID: " + auth.user_id);

                  

                    var hj = response.Content;
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var myresponse = js.Deserialize<dynamic>(hj);
                    var ret = myresponse["user"]; //data


                    ObjUtil.WriteErrorLog("Staffvalidation",
                      "Response: " + hj.ToString() + Environment.NewLine + "****" + Environment.NewLine +
                      "Response Msg: " + myresponse["message"] + "|00000" + respCode + Environment.NewLine + "*****" + Environment.NewLine + "username: " + auth.user_id);


                    if (myresponse != null)
                    {
                        respCode = 00;
                        respMsg = myresponse["message"];
                       
                        //respMsg = myresponse["success"];
                        


                        if (myresponse["user"] != null)
                        {
                            staffdetail = new StaffDetails()
                            {
                                role = myresponse["user"]["jobTitle"],
                                branch = branch_code,
                                staff_name = myresponse["user"]["userPrincipalName"],
                                fullname = myresponse["user"]["displayName"],
                                usertype = 1,
                                response_code = respCode,
                                response_description = respMsg,
                            };

                        }
                        else
                        {
                            ObjUtil.WriteErrorLog("Staffvalidation",
                              "Response: " + hj.ToString() + Environment.NewLine + "****" + Environment.NewLine +
                              "Response Msg: " + myresponse["message"] + "|1111" + respCode + Environment.NewLine + "*****" + Environment.NewLine + "username: " + auth.user_id);

                            return staffdetail;
                        }
                    }
                    else
                    {
                        staffdetail = new StaffDetails()
                        {
                            role = "",
                            branch = "",
                            staff_name = "",
                            response_code = 1,
                            response_description = "No record is returned from AD"
                            //response_description = response.StatusDescription
                        };
                    }



                }

            }
            catch (Exception ex)
            {
                ObjUtil.WriteErrorLog("Staffvalidation error",
                           ex.Message + Environment.NewLine + "************" + Environment.NewLine +
                           ex.StackTrace + Environment.NewLine + "***********" + Environment.NewLine + ex.InnerException);

                staffdetail = new StaffDetails()
                {
                    role = "",
                    branch = "",
                    staff_name = "",
                    response_code = 4,
                    response_description = "An error occured on the server"
                    //response_description = response.StatusDescription
                };
            }




            return staffdetail;
        }


        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.Route("api/DraxlaConnect/AuthCodeDelivery")]
        public string AuthCodeDelivery(Authcode authcode)
        {
            string authcodeapiurl = "";
            //StaffDetails staffdetail = new StaffDetails();
            string accounttype = "";
            string myaccesscode = "";
            //var body = "";

            if(authcode == null)
            {
                ObjUtil.WriteErrorLog("AuthCodeDelivery message 00000000---00000",
"message: " + "11111111111" + Environment.NewLine + "****" + Environment.NewLine +
"Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");

            }
            else
            {
                ObjUtil.WriteErrorLog("AuthCodeDelivery message 00000000---1111111",
"message: " + "2222222222" + Environment.NewLine + "****" + Environment.NewLine +
"Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");

            }



            //authcode.message_body = "The authorization code value is " + authcode.authorizationCode;
            //ObjUtil.WriteErrorLog("AuthCodeDelivery message",
            //    "message: " + "" + Environment.NewLine + "****" + Environment.NewLine +
            //    "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");



            try
            {

                if (authcode.email_address.Trim() != "")
                {
                    SendEmail(authcode);
                }

                if (authcode.telephone.Trim() != "")
                {
                    SendSMS(authcode);
                }
                

                  
                //string resp = "";
                //Random rx = new Random();
                //int rand = rx.Next(10001, 20000);
                //string tran_id = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + '-' + rand;
                //Encryption ect = new Encryption();

                //if (authcode.accessCode == null)
                //{
                //    authcodeapiurl = ConfigurationManager.AppSettings["retail_auth_code_delivery"].ToString(CultureInfo.InvariantCulture);
                //}
                //else
                //{
                //    authcodeapiurl = ConfigurationManager.AppSettings["corporate_auth_code_delivery"].ToString(CultureInfo.InvariantCulture);
                //    myaccesscode = ect.Encrypt(authcode.accessCode);
                //}


                //string userid = ect.Encrypt(authcode.userId);
                //string requestid = ect.Encrypt(tran_id);
                //string myauthcode = ect.Encrypt(authcode.authorizationCode);



                ////string token = GetToken();
                //if (token.Trim() == "")
                //{
                //    token = GetToken();
                //}

                //if (token.Trim() == "")
                //{
                //    respCode = "-1";
                //    respMsg = "Token cannot be retrieved at this time. Please retry.";

                //}
                //else
                //{

                //    var client = new RestClient(authcodeapiurl);
                //    client.Timeout = -1;
                //    var request = new RestRequest(Method.POST);
                //    request.AddHeader("Authorization", token);
                //    request.AddHeader("Content-Type", "application/json");

                //    JObject body = new JObject();

                //    if (authcode.accessCode == null)
                //    {
                //        accounttype = "2";
                //        body.Add("userId", userid);
                //    }
                //    else
                //    {
                //        accounttype = "1";
                //        body.Add("accessCode", myaccesscode);
                //        body.Add("userId", userid);
                //    }

                //    body.Add("requestId", requestid);
                //    body.Add("authorizationCode", myauthcode);
                //    body.Add("channel", "TP-ETOKEN");
                //    body.Add("accountType", accounttype);



                //    request.AddParameter("application/json", body, ParameterType.RequestBody);
                //    IRestResponse response = client.Execute(request);
                //    Console.WriteLine(response.Content);


                //    var hj = response.Content;
                //    JavaScriptSerializer js = new JavaScriptSerializer();
                //    var myresponse = js.Deserialize<dynamic>(hj);
                //    // var ret = myresponse["data"];
                //    if (myaccesscode != null)
                //    {
                //        respCode = myresponse["responseCode"];
                //        respMsg = myresponse["responseDescription"];
                //    }
                //    else
                //    {
                //        respCode = "1";
                //        respMsg = "No response";
                //    }

                //    ObjUtil.WriteErrorLog("AuthCodeDelivery",
                //         "Resp code: " + respCode + Environment.NewLine + "****" + Environment.NewLine +
                //         "Response Msg: " + respMsg + Environment.NewLine + "*****" + Environment.NewLine + " " + "");

                //}
            }
            catch (Exception ex)
            {
                respCode = "-1";
                respMsg = "An error occured";

                ObjUtil.WriteErrorLog("AuthCodeDelivery Error",
                 "Resp code: " + ex.ToString() + Environment.NewLine + "****" + Environment.NewLine +
                 "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");

            }
            return respCode + "|" + respMsg;

        }




        
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.Route("api/DraxlaConnect/GetTokenDetailsFromBlob")]
        public TokenDetails GetTokenDetailsFromBlob(TokenDetails tokenblob)
        {
           //string g = "FEB2925637AUTHENT     10AvKARAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGAAIAAAAgICAAAQWjuh+oq/9kCGwa8e4s7T05WAhDe7JT2RAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAWyZSKL+oI80EAAQAAAABAE1PQjQxAIIAAAAAAAAAAACw2nn48rqyCxiPZMUw1zq4";

            //ObjUtil.WriteErrorLog("isTokenLock",
            //"threshod: " + "" + Environment.NewLine + "************" + Environment.NewLine +
            // "count: " + "" + "" + Environment.NewLine + "***********" + Environment.NewLine + "");




            TokenDetails tk = new TokenDetails();
            try
            {
                string returnMsg = "";
                string islock = "";
                string mythreshold = ConfigurationManager.AppSettings["lockthreshood"].ToString(CultureInfo.InvariantCulture);

                //string tokenblob = "FDQ4365313DIAMONDSOFT 100BSDAQEAEAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAIAAIAAAAgICAAAAhHL9d+4K4Ng7nLwkPNXIMLVplKPKse/2PBg6dhAAAAAAIAAAAAAAAAAAAAAAAAAAAAAAAAfBOff3YIrXwAAAAAAAAAAERBTDEwAIIAAQBjAQEAAADzTg+Fb4zjOBkxWzEfrvaJ";
                //string tokenblob = "FDQ4365313RO        0110AtCAAAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGAAIAAAAAAAAAAADaHXVKD51NnKVH / DWLIPtVambd1yw7Z5cAAAAAAAAAAAMAAAAAAAAAAAAAAAAAAAAAAAAAYGk9OfgMzK0AAAAAAAAAAFRZUDAzAIIAAAAAAAAAAAAEtY3aLpGwp7bmypKRWWVX";
                //string g = "FEE3259856AUTHENT     10AvKARAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGAAIAAAAgICAAAQWoNHMCHoMYl6MTmD5fILBYnbPp2MhJvOOAUMFjYAEAAAIAAAAAAAAAAAAAAAAAAAAAAAAA81AjlMIzm6YEAAQAAAABAE1PQjQxAJIAAAAAAAEAAABi911kZPXpupnu8DY+A7/U";
                //string g = "FEE3259856AUTHENT     10AvKARAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGAAIAAAAgICAAAQX0XXZPCky/bXlWE9NiFWy+ljw+ZRVwpdsAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA9/CEhCMCAd8EAAQAAAABAE1PQjQxAIIAAAAAAAAAAAAFi7MitebouhAhjr0yBFNa";

                AAL2Wrap b = new AAL2Wrap();
                AAL2Wrap.TDigipassInfo pg = b.AAL2GetTokenInfo(tokenblob.blob);
                //AAL2Wrap.TDigipassInfo pg = b.AAL2GetTokenInfo(g);
                string a1 = pg.CodeWord;
                string a2 = pg.ErrorCount;
                int a2E = Convert.ToInt32(a2);
                int threshold = Convert.ToInt32(mythreshold);

                //ObjUtil.WriteErrorLog("isTokenLockvvvvv",
                //"threshod: " + threshold + Environment.NewLine + "************" + Environment.NewLine +
                // "count: " + a2E + "" + Environment.NewLine + "***********" + Environment.NewLine + "");


                if (a2E < threshold)
                {
                    islock = "No";

                //    ObjUtil.WriteErrorLog("isTokenLockNo",
                //"threshold: " + threshold + Environment.NewLine + "************" + Environment.NewLine +
                // "count: " + a2E + "" + Environment.NewLine + "***********" + Environment.NewLine + "");

                }
                else
                {
                //    ObjUtil.WriteErrorLog("isTokenLockYes",
                //"threshold: " + threshold + Environment.NewLine + "************" + Environment.NewLine +
                // "count: " + a2E + "" + Environment.NewLine + "***********" + Environment.NewLine + "");

                    islock = "Yes";
                }


                int a3 = pg.GetHashCode();
                Object a4 = pg.GetType();
                Object a4a = a4.Equals(a4);
                Object a4b = a4.GetType();
                string a5 = pg.LastTimeShift;
                string a6 = pg.LastTimeUsed;
                string a7 = pg.MaxInputFields;
                string a8 = pg.ResponseChecksum;
                int a9 = Convert.ToInt32(pg.ResponseLength);
                string a10 = pg.ResponseType;
                int a11 = Convert.ToInt32(pg.TimeStepUsed);
                string a12 = pg.TokenModel;
                string a13 = pg.TripleDes;
                int a14 = Convert.ToInt32(pg.UseCount);

                returnMsg = "Successful|Response Length=" + a9 + " |Error Count= " + a2 + "| Response Type= " + a10 + "| Time Step = " + a11 + " |LastTimeUsed =" + a6.ToString() + "|UseCount=" + a14 + "|Token model = " + a12 + "|Tripple Des=" + a13 + " | Last Timeshift = " + a5;


                tk = new TokenDetails()
                {
                    code_word = a1,
                    error_count = a2E,
                    is_locked = islock,
                    last_time_shift = a5,
                    last_time_used = a6,
                    max_input_fields = a7,
                    response_checksum = a8,
                    response_length = a9,
                    response_type = a10,
                    time_step_used = a11,
                    token_model = a12,
                    triple_des = a13,
                    use_count = a14
                };
            }
            catch(Exception ex)
            {
                ObjUtil.WriteErrorLog("error",
             "threshold: " + ex.ToString() + Environment.NewLine + "************" + Environment.NewLine +
              "" + "" + "" + Environment.NewLine + "***********" + Environment.NewLine + "");

            }


            return tk;
        }


  

        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.Route("api/DraxlaConnect/SyncAndUnlockUser")]
        public List<TokenDetails> SyncAndUnlockUser([FromBody] List<TokenDetails> tokendetail)
        {
            List<TokenDetails> resp = new List<TokenDetails>();
            TokenDetails td = new TokenDetails();
            if (tokendetail.First().operation_type == "sync")
            {
                foreach(var n in tokendetail)
                {
                    td = TokenSync(n);
                    //if(td.return_code == 0)
                    //{
                        resp.Add(new TokenDetails
                        {
                          blob = td.blob,
                          sequence_no = n.sequence_no,
                          return_msg = td.return_msg,
                          return_code = td.return_code
                          
                        });
                    //}
                }
            }
            else
            {
                foreach (var n in tokendetail)
                {
                    td = unlockUserToken(n);
                    //if (td.return_code == 0)
                    //{
                        resp.Add(new TokenDetails
                        {
                            blob = td.blob,
                            sequence_no = n.sequence_no,
                            return_msg = td.return_msg,
                            return_code = td.return_code
                        });
                    //}
                }
            }


            return resp;
        }





       

        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.Route("api/DraxlaConnect/TokenValidator")]
        public List<TokenDetails> TokenValidator(List<TokenDetails> tokendetail)
        {

            ObjUtil.WriteErrorLog("DR got herer",
                               "11111111111111111111" + Environment.NewLine + "************" + Environment.NewLine +
                               "aaaaaaaaaaa" + Environment.NewLine + "***********" + Environment.NewLine + "");

            // return "false";

            //List<TokenDetails> resp = new List<TokenDetails>();


            try
            {
                TokenDetails td = new TokenDetails();
                resp = new List<TokenDetails>();
                int count = 0;
                foreach (var n in tokendetail)
                {

                    td = ValidateToken(n);
                    if (td != null)
                    {
                        if (td.return_code == 0)
                        {
                            //resp = null;
                            resp = new List<TokenDetails>();
                            resp.Add(new TokenDetails
                            {
                                blob = td.blob,
                                return_code = td.return_code,
                                return_msg = td.return_msg,
                                sequence_no = n.sequence_no
                            });

                            break;
                        }
                        else
                        {
                            resp.Add(new TokenDetails
                            {
                                blob = td.blob,
                                return_code = td.return_code,
                                return_msg = td.return_msg,
                                sequence_no = n.sequence_no
                            });
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                ObjUtil.WriteErrorLog("TokenValidator",
                            ex.ToString() + Environment.NewLine + " * ***********" + Environment.NewLine +
                            "" + Environment.NewLine + "***********" + Environment.NewLine + "");

            }

            ObjUtil.WriteErrorLog("got RESPONSE---------",
                              resp[0].return_msg + Environment.NewLine + "************" + Environment.NewLine +
                              resp[0].return_code + Environment.NewLine + "***********" + Environment.NewLine + "");


            return resp;
        }



        public TokenDetails unlockUserToken(TokenDetails digi)
        {
            string mythreshold = ConfigurationManager.AppSettings["lockthreshood"].ToString(CultureInfo.InvariantCulture);
            string itimewindow = ConfigurationManager.AppSettings["iTimeWindow"].ToString(CultureInfo.InvariantCulture);
            TokenDetails myresp = new TokenDetails();
            try
            {

                AAL2Wrap oAAL2Wrap = new AAL2Wrap
                {
                    KParams =
                    {
                      ParmCount = 19,
                      ITimeWindow = Convert.ToInt32(itimewindow),
                      STimeWindow = 24,
                      DiagLevel = 0,
                      GMTAdjust = 0,
                      CheckChallenge = 0,
                      IThreshold = Convert.ToInt32(mythreshold),
                      SThreshold = 1,
                      ChkInactDays = 0,
                      DeriveVector = 0,
                      SyncWindow = 6,
                      OnLineSG = 1,
                      EventWindow = 30,
                      HSMSlotId = 0,
                      StorageKeyId = 0,
                      TransportKeyId = 0x7FFFFF,
                      StorageDeriveKey1 = 0,
                      StorageDeriveKey2 = 0,
                      StorageDeriveKey3 = 0,
                      StorageDeriveKey4 = 0
                    }
                };

                var cc = AAL2Wrap.ERROR_COUNT;
                var dp = "";
                dp = digi.blob;
                //******************************************* Unlock token Section
                int ret = 0;

                //var ff0 = oAAL2Wrap.AAL2GetTokenInfoEx(dp);
                var ff = oAAL2Wrap.AAL2GetTokenProperty(dp, cc, ref ret);
                //var resp = oAAL2Wrap.AAL2SetTokenProperty(ref dp, cc, 0) == 0 ? true : false;
                var resp = oAAL2Wrap.AAL2SetTokenProperty(ref dp, cc, 0);
                var msg = oAAL2Wrap.getError(resp);

                myresp = new TokenDetails();
                myresp.blob = dp;
                myresp.return_code = resp;
                myresp.return_msg = msg;

                // return "false";
                //throw;
            }
          
            catch (Exception ex)
            {

                ObjUtil.WriteErrorLog("",
                                       ex.Message + Environment.NewLine + "************" + Environment.NewLine +
                                       ex.StackTrace + Environment.NewLine + "***********" + Environment.NewLine + ex.InnerException);

            }

            return myresp;
        }
   
        public string DraxlaCallOTP(string soap)
        {
            //http://maxutserver101.eastus.cloudapp.azure.com/Zenith_DraxlaExpress/Service.asmx
            string draxlaurl = ConfigurationManager.AppSettings["DraxlaOTP"].ToString(CultureInfo.InvariantCulture);

            string resp = "";

            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(draxlaurl);
                //HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(draxlaurl);

                myRequest.ContentType = "text/xml;charset=\"utf-8\"";
                myRequest.Method = "POST";

                using (Stream stm = myRequest.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(stm))
                    {
                        stmw.Write(soap);
                    }
                }

                //myRequest.Headers["Cookie"] = "TemplateID=4236;WorkOrderID=12914;SchemeNumber=000001;user_id=0";

                HttpWebResponse response = myRequest.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());

                resp = reader.ReadToEnd();
                //Response.Write(reader.ReadToEnd());
                reader.Close();
            }
            catch (Exception ex)
            {
                resp = ex.ToString();
            }


            return resp;
        }

        public string DraxlaCallOTPPWD(string soap)
        {
            //http://maxutserver101.eastus.cloudapp.azure.com/Zenith_DraxlaExpress/Service.asmx
            string draxlaurl = ConfigurationManager.AppSettings["DraxlaOTPPWD"].ToString(CultureInfo.InvariantCulture);

            string resp = "";

            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(draxlaurl);
                myRequest.ContentType = "text/xml;charset=\"utf-8\"";
                myRequest.Method = "POST";

                using (Stream stm = myRequest.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(stm))
                    {
                        stmw.Write(soap);
                    }
                }

                //myRequest.Headers["Cookie"] = "TemplateID=4236;WorkOrderID=12914;SchemeNumber=000001;user_id=0";

                HttpWebResponse response = myRequest.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());

                resp = reader.ReadToEnd();
                //Response.Write(reader.ReadToEnd());
                reader.Close();
            }
            catch (Exception ex)
            {
                resp = ex.ToString();
            }


            return resp;
        }

        string GetToken()
        {
            string resp = "";
            string tokenurl = ConfigurationManager.AppSettings["token_url"].ToString(CultureInfo.InvariantCulture);


            ObjUtil.WriteErrorLog("GetToken",
                          "Get to getToken: " + "" + Environment.NewLine + "****" + Environment.NewLine +
                          "" + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");


            Random rx = new Random();
            int rand = rx.Next(10001, 20000);
            string tran_id = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + '-' + rand;

            try
            {

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                          | SecurityProtocolType.Tls
                          | SecurityProtocolType.Tls11
                          | SecurityProtocolType.Tls12;


                var client = new RestClient(tokenurl);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);

                request.AddHeader("Content-Type", "application/json");

                JObject jObjectbody = new JObject();

                //prod
                //jObjectbody.Add("username", "d25e1925-c074-45f2-93bd-05c84cd72856");
                //jObjectbody.Add("password", "ODg3M2QtMjI3MC0NTk3MTMwZDAtYzhlMi00Y2M4LWE2N0Mjg5LTg0MGUtNTY1YzQz");
                
                //UAT
                jObjectbody.Add("username", "d25e1925-c074-45f2-93bd-05c84cd72856");
                jObjectbody.Add("password", "ODg3M2QtMjI3MC0NTk3MTMwZDAtYzhlMi00Y2M4LWE2N0Mjg5LTg0MGUtNTY1YzQz");


                request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);


                ObjUtil.WriteErrorLog("Token Testing 222222 Today",
                  "Payload: " + jObjectbody + Environment.NewLine + "****" + Environment.NewLine +
                  "URL: " + tokenurl + " " + Environment.NewLine + " * ****" + Environment.NewLine + " " + "");



                var hj = response.Content;
                JavaScriptSerializer js = new JavaScriptSerializer();
                var myresponse = js.Deserialize<dynamic>(hj);


                ObjUtil.WriteErrorLog("Token Testing",
                    "Token: " + hj.ToString() + Environment.NewLine + "****" + Environment.NewLine +
                    "Response Msg: " + response.StatusCode + Environment.NewLine + "*****" + Environment.NewLine + " " + "");

                if (myresponse["accessToken"] != "")
                {


                    ObjUtil.WriteErrorLog("xxxxxxxxx",
                          "Resp code: " + "" + Environment.NewLine + "****" + Environment.NewLine +
                           "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");

                    //var ret = myresponse["userName"];
                    resp = myresponse["accessToken"];

                    ObjUtil.WriteErrorLog("yyyyyy",
      "Resp code: " + "" + Environment.NewLine + "****" + Environment.NewLine +
       "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");

                }
                else
                {

                    //var ret = myresponse["ResponseCode"];
                    //resp = myresponse["ResponseDescription"];

                    ObjUtil.WriteErrorLog("GetToken",
                          "Resp code: " + myresponse["responseCode"] + Environment.NewLine + "****" + Environment.NewLine +
                           "Response Msg: " + myresponse["responseDescription"] + Environment.NewLine + "*****" + Environment.NewLine + " " + "");
                
                
                }

                //ObjUtil.WriteErrorLog("Token",
                //     "Token: " + resp + Environment.NewLine + "****" + Environment.NewLine +
                //     "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");

            }
            catch (Exception ex)
            {
                ObjUtil.WriteErrorLog("SV 444444Error",
             "Token: " + ex.ToString() + Environment.NewLine + "****" + Environment.NewLine +
              "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");


                ObjUtil.WriteErrorLog("Token Error",
                     "Token: " + ex.ToString() + Environment.NewLine + "****" + Environment.NewLine +
                     "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");

            }

            ObjUtil.WriteErrorLog("SV 555555rror",
          "Token resp: " + resp + Environment.NewLine + "****" + Environment.NewLine +
           "Response Msg: " + "" + Environment.NewLine + "*****" + Environment.NewLine + " " + "");


            return resp;
        }

        public returnDigipassJson CommitDpxFile(String dpxFileName, String trsnsportKey, string uploader, ref string msg, ref string tokencount)
        {
            List<digipass> mydigipass = new List<digipass>();
            string staticvector = "", msgvec = "";
            string tokentype = "H";
            int ret = 0;

            try
            {
                string batchno = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;

                Vasco.AAL2Wrap.TDigipass[] TokenDatas;
                dpxExplore = new AAL2Wrap();

                String[] dpBlobs;
                int appCount = 9999;
                int tokenCount = 0;


                dpxExplore.AAL2DPXInit(
                                          dpxFileName,
                                               //@"C:\ApplicationsDevelopment\mobileproject\DP4MOBILE\DP4MOB_4.6.4\DPXs\AES (RO_CR_SG) + No PIN\DIGIPASS_for_Mobile_DEMO_AES_NoPin.dpx",
                                               trsnsportKey,
                                               ref appCount,
                                               ref tokenCount);


                ret = dpxExplore.getRetCode();

                if (ret != 0)
                {
                    msg = dpxExplore.getDPXError(ret);


                }
                else
                {
                    tokencount = tokenCount.ToString();
                    dpBlobs = new string[appCount * tokenCount];

                    int genStaticVector = dpxExplore.AAL2DPXGetStaticVector();
                    msg = dpxExplore.getError(genStaticVector);
                    staticvector = "";
                    if (genStaticVector == 0)
                    {
                        staticvector = dpxExplore.getStaticVector();
                        tokentype = "S";
                    }


                    int genMessageVector = dpxExplore.AAL2DPXGetMessageVector();
                    msg = dpxExplore.getError(genMessageVector);
                    if (genMessageVector == 0)
                    {
                        msgvec = dpxExplore.getMessageVector();
                    }

                    for (int i1 = 0; i1 < tokenCount; i1++)
                    {
                        Vasco.Response.RespDPXGetTokenBlobsEx p = dpxExplore.AAL2DPXGetTokenBlobsEx();
                        int retVal = dpxExplore.getRetCode();
                        if (retVal != 100)
                        {
                            msg = dpxExplore.getError(retVal);
                        }


                        for (int i2 = 0; i2 < appCount; i2++)
                        {
                            int proceed = 0;
                            string[] modeb = p.getAuthMode();
                            if (msgvec == "")
                            {
                                if (modeb[i2] != "RO")
                                {
                                    proceed = 1;
                                }
                            }

                            //string[] modeb = p.getAuthMode();

                            if (proceed == 0)
                            {
                                string[] blobb = p.getDpData();
                                byte[][] blobbyte = p.getbDpData();
                                // string[] modeb = p.getAuthMode();
                                string[] app = p.getSerialAppl();

                                mydigipass.Add(new digipass
                                {
                                    serial_no = p.getSerial(),
                                    //blob = EncryptData(blobb[0]),
                                    blob = blobb[0],
                                    blob_backup = EncryptData(blobb[0]),
                                    byte_blob = blobbyte[0],
                                    dp_type = p.getTokenType(),
                                    activation_vector = p.getActivationVector(),
                                    sequence_no_threshold = p.getSeqNumThreshold(),
                                    token_state = 0,
                                    user_type = 1,
                                    bind_status = 0,
                                    created_by = uploader,
                                    created_date = DateTime.Now,
                                    
                                    mode = modeb[i2],
                                    token_type = tokentype,
                                    vds_appl_name = app[0],
                                    batch_no = batchno
                                });;
                            }

                        }
                    }
                    //---------------------------------------
                    ret = dpxExplore.AAL2DPXClose();
                    if (ret != 0)
                    {
                        msg = dpxExplore.getError(ret);
                    }
                    else
                    {
                        msg = dpxExplore.getError(ret);
                    }
                }


            }
            catch (Exception ex)
            {
                // ObjUtil.WriteErrorLog("ImportBlobsFromDPX", ex.Message + Environment.NewLine + ex.InnerException);
                //return null;
            }

            returnDigipassJson hg = new returnDigipassJson();
            hg.status = ret.ToString();
            hg.message = msg;
            hg.static_vector = staticvector;
            hg.message_vector = msgvec;
            hg.transport_key = trsnsportKey;
            hg.token_type = tokentype;
            hg.data = mydigipass;

            return hg;

        }

        public TokenDetails TokenSync(TokenDetails digi)
        {
            string mythreshold = ConfigurationManager.AppSettings["lockthreshood"].ToString(CultureInfo.InvariantCulture);
            string itimewindow = ConfigurationManager.AppSettings["iTimeWindow"].ToString(CultureInfo.InvariantCulture);

            TokenDetails myresp = new TokenDetails();

            try
            {
                AAL2Wrap oAAL2Wrap = new AAL2Wrap
                {
                    KParams =
                    {
                      ParmCount = 19,
                      ITimeWindow =  Convert.ToInt32(itimewindow),
                      STimeWindow = 24,
                      DiagLevel = 0,
                      GMTAdjust = 0,
                      CheckChallenge = 0,
                      IThreshold = Convert.ToInt32(mythreshold),
                      SThreshold = 1,
                      ChkInactDays = 0,
                      DeriveVector = 0,
                      SyncWindow = 6,
                      OnLineSG = 1,
                      EventWindow = 30,
                      HSMSlotId = 0,
                      StorageKeyId = 0,
                      TransportKeyId = 0x7FFFFF,
                      StorageDeriveKey1 = 0,
                      StorageDeriveKey2 = 0,
                      StorageDeriveKey3 = 0,
                      StorageDeriveKey4 = 0
                    }
                };

                var cc = AAL2Wrap.ERROR_COUNT;
                var dp = "";
                string blob = "", tkntyp = "", sno = "";
                dp = digi.blob;
   
                //******************************************* Unlock token Section

                    int ret = 0;

                    //var ff0 = oAAL2Wrap.AAL2GetTokenInfoEx(dp);

                var ff = oAAL2Wrap.AAL2GetTokenProperty(dp, cc, ref ret);
                var resp = oAAL2Wrap.AAL2ResetTokenInfo(ref dp);

                var msg = oAAL2Wrap.getError(resp);
                // var oo = oAAL2Wrap.AAL2GetTokenInfoEx(dp);
                // UpdateDPData(sno, dp);

                string newblob = "";      
                 newblob = dp;

                //if (tkntyp == "H")
                //{
                    digi.blob = newblob;
                   // userid = digi.user_id;
                //}

                myresp = new TokenDetails();
                myresp.blob = newblob;
                myresp.return_code = resp;
                myresp.return_msg = msg;

            }
            catch (Exception ex)
            {
                ObjUtil.WriteErrorLog("",
                                    ex.Message + Environment.NewLine + "************" + Environment.NewLine +
                                    ex.StackTrace + Environment.NewLine + "***********" + Environment.NewLine + ex.InnerException);

               // return "false";
                //throw;
            }
            return myresp;


        }

        //main validator
        public TokenDetails ValidateToken(TokenDetails digi)
        {
            string mythreshold = ConfigurationManager.AppSettings["lockthreshood"].ToString(CultureInfo.InvariantCulture);
            string itimewindow = ConfigurationManager.AppSettings["iTimeWindow"].ToString(CultureInfo.InvariantCulture);

            TokenDetails myresp = new TokenDetails();

            ObjUtil.WriteErrorLog("Testing101",
                            digi.blob + Environment.NewLine + "************" + Environment.NewLine +
                            "" + Environment.NewLine + "***********" + Environment.NewLine + "OTP: " + digi.OTP.Trim());


            try
            {
                AAL2Wrap oAAL2Wrap = new AAL2Wrap
                {
                    KParams =
                    {
                      ParmCount = 19,
                      ITimeWindow =  Convert.ToInt32(itimewindow),
                      STimeWindow = 24,
                      DiagLevel = 0,
                      GMTAdjust = 0,
                      CheckChallenge = 0,
                      IThreshold = Convert.ToInt32(mythreshold),
                      SThreshold = 1,
                      ChkInactDays = 0,
                      DeriveVector = 0,
                      SyncWindow = 6,
                      OnLineSG = 1,
                      EventWindow = 30,
                      HSMSlotId = 0,
                      StorageKeyId = 0,
                      TransportKeyId = 0x7FFFFF,
                      StorageDeriveKey1 = 0,
                      StorageDeriveKey2 = 0,
                      StorageDeriveKey3 = 0,
                      StorageDeriveKey4 = 0
                    }
                };

                var cc = AAL2Wrap.ERROR_COUNT;
                var dp = "";
                string blob = "", tkntyp = "", sno = "";
                dp = digi.blob;

                int ret = oAAL2Wrap.AAL2VerifyPassword(ref dp, digi.OTP.Trim(), null);
                string response = oAAL2Wrap.getError(ret);

                ObjUtil.WriteErrorLog("DR got herer 3333333333333",
                           "otp: " + digi.OTP.Trim() + " " + Environment.NewLine + "************" + Environment.NewLine +
                           "ret: " + ret + " " + Environment.NewLine + "***********" + Environment.NewLine + "");

               
                if (ret == 0)
                {
                    resp = null;
                }

                myresp = new TokenDetails();
                myresp.blob = dp;
                myresp.return_code = ret;
                myresp.return_msg = response;

            }
            catch (Exception ex)
            {
                ObjUtil.WriteErrorLog("",
                                    ex.Message + Environment.NewLine + "************" + Environment.NewLine +
                                    ex.StackTrace + Environment.NewLine + "***********" + Environment.NewLine + ex.InnerException);

                // return "false";
                //throw;
            }
           

            ObjUtil.WriteErrorLog("Testing102",
                            myresp.return_code + Environment.NewLine + "************" + Environment.NewLine +
                            myresp.return_msg + Environment.NewLine + "***********" + Environment.NewLine + "");

            return myresp;

        }

        public static string EncryptData(string clearText)
        {
            string EncryptionKey = "Password@#21";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        /*
        [System.Web.Http.AcceptVerbs("POST")]
        [System.Web.Http.Route("api/DraxlaConnect/TestSage")]
        public UserEnumeration TestSage()
        {

            UserEnumeration userenum = new UserEnumeration();

            string resp = "";
            Random rx = new Random();
            int rand = rx.Next(10001, 20000);
            string tran_id = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + '-' + rand;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                      | SecurityProtocolType.Tls
                      | SecurityProtocolType.Tls11
                      | SecurityProtocolType.Tls12;

            var client = new RestClient("https://resellers.accounting.sageone.co.za/api/2.0.0/seconnect/Account/Get?apikey={7257D855-27C7-477A-BA58-2EB88BDA4728}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            var body = @"";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);


            var hj = response.Content;
            JavaScriptSerializer js = new JavaScriptSerializer();
            var myresponse = js.Deserialize<dynamic>(hj);
            // var ret = myresponse["data"];
            int respCode = myresponse["responseCode"];
            string respMsg = myresponse["responseDescription"];

            if (myresponse["data"] != null)
            {
                userenum = new UserEnumeration()
                {
                    customerFirstName = myresponse["data"]["customerFirstName"],
                    customerLastName = myresponse["data"]["customerLastName"],
                    customerMiddleName = myresponse["data"]["customerLastName"],
                    customerEmail = myresponse["data"]["customerEmail"],
                    customerPhoneNo = myresponse["data"]["customerPhoneNo"],
                    responseCode = respCode.ToString(),
                    customerMsg = respMsg

                };

            }
            else
            {
                userenum = new UserEnumeration()
                {
                    customerFirstName = "",
                    customerLastName = "",
                    customerMiddleName = "",
                    customerEmail = "",
                    customerPhoneNo = "",
                    responseCode = respCode.ToString(),
                    customerMsg = respMsg

                };
            }


            return userenum;
        }
        */

         public static class ObjUtil
        {

            public static string ConfigPath = ConfigurationManager.AppSettings["errorPath"].ToString(CultureInfo.InvariantCulture);


            public static void WriteErrorLog(string subject, string message)
            {


                if (!System.IO.Directory.Exists(ConfigPath))
                {
                    System.IO.Directory.CreateDirectory(ConfigPath);
                }

                //check the file
                var fs = new FileStream(ConfigPath + "\\Errlog" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var s = new StreamWriter(fs);
                s.Close();
                fs.Close();

                //log it
                var fs1 = new FileStream(ConfigPath + "\\Errlog" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append, FileAccess.Write);
                var s1 = new StreamWriter(fs1);
                s1.WriteLine("Title: " + subject);
                s1.WriteLine("Message: " + message);
                s1.WriteLine("Date/Time: " + DateTime.Now);
                s1.WriteLine("================================================");
                s1.Close();
                fs1.Close();
            }

            public static void AcctivityLog(string cif, string serialnumber, string response, string otp, string ops)
            {


                if (!System.IO.Directory.Exists(ConfigPath))
                {
                    System.IO.Directory.CreateDirectory(ConfigPath);
                }

                //check the file
                var fs = new FileStream(ConfigPath + "\\ActivityLog_" + serialnumber + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                var s = new StreamWriter(fs);
                s.Close();
                fs.Close();

                //log it
                var fs1 = new FileStream(ConfigPath + "\\ActivityLog_" + serialnumber + "_" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append, FileAccess.Write);
                var s1 = new StreamWriter(fs1);
                s1.WriteLine("SerialNumber: " + serialnumber);
                s1.WriteLine("CIF: " + cif);
                s1.WriteLine("Validation Response: " + response);
                s1.WriteLine("OTP: " + otp);
                s1.WriteLine("TRANS OPERATION: " + ops);
                s1.WriteLine("Date/Time: " + DateTime.Now);
                s1.WriteLine("================================================");
                s1.Close();
                fs1.Close();
            }
        }


    }
}