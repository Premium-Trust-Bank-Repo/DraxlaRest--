using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using VacmanBoundContext;
using Vasco;
using System.IO;

namespace VacmanBoundRepo
{
    public class VacmanEngine
    {
        private readonly VacmanRepoUOW.DigipassRepo _repoDigipass;
        //private readonly VacmanRepoUOW.TokenActivationRepo _repoTokenactivation;
      

        AAL2Wrap oAAL2Wrap;
        string is_blob_encrypt = ConfigurationManager.AppSettings["isblobencrypt"].ToString(CultureInfo.InvariantCulture);
        int lock_threshood = Convert.ToInt32(ConfigurationManager.AppSettings["lockthreshood"].ToString(CultureInfo.InvariantCulture));
        int itoken_window = Convert.ToInt32(ConfigurationManager.AppSettings["itokenWindow"].ToString(CultureInfo.InvariantCulture));


        VacmanContext context = new VacmanContext();
        int ret = 0;
        string msg = "";

        VacmanContext ws = new VacmanContext();

        public VacmanEngine()
        {
            _repoDigipass = new VacmanRepoUOW.DigipassRepo();
            //_repoTokenactivation = new VacmanRepoUOW.TokenActivationRepo();
        }



        //public  string ActivateToken(string sno, string userid, string blob, string derivationcode, string deviceType)
        //public string ActivateToken(string sno, string userid, string blob, string derivationcode)
        //{


        //    try
        //    {
        //        //int ret = 0;
        //        oAAL2Wrap = new AAL2Wrap
        //        {
        //            KParams =
        //            {
        //                ParmCount = 19,
        //                //ITimeWindow = 30,
        //                ITimeWindow = itoken_window,
        //                STimeWindow = 24,
        //                DiagLevel = 0,
        //                GMTAdjust = 0,
        //                CheckChallenge = 0,
        //                IThreshold = lock_threshood,
        //                SThreshold = 1,
        //                ChkInactDays = 0,
        //                DeriveVector = 0,
        //                SyncWindow = 6,
        //                OnLineSG = 1,
        //                EventWindow = 30,
        //                HSMSlotId = 0,
        //                StorageKeyId = 0,
        //                TransportKeyId = 0x7FFFFF,
        //                StorageDeriveKey1 = 0,
        //                StorageDeriveKey2 = 0,
        //                StorageDeriveKey3 = 0,
        //                StorageDeriveKey4 = 0
        //            }
        //        };


        //        var actualblob = "";
        //        if (is_blob_encrypt == "Yes")
        //        {
        //            actualblob = _repoDigipass.DecryptTokenBlob(blob);

        //        }
        //        else
        //        {
        //            actualblob = blob;
        //        }

        //        var temp = actualblob;

        //        string[] dpArray = new string[] { actualblob };

        //        AAL2Wrap wrapper = new AAL2Wrap();
        //        uint vv = 0;
        //        ret = wrapper.AAL2DeriveTokenBlobs(dpArray, (short)dpArray.Length, null, derivationcode, vv);

        //        msg = wrapper.getError(ret);

        //        //ret = 0;


              


        //        if (ret == 0)
        //        {
        //            string newblob = "";

        //            if (is_blob_encrypt == "Yes")
        //            {
        //                newblob = _repoDigipass.EncryptTokenBlob(dpArray[0]);
        //            }
        //            else
        //            {
        //                newblob = dpArray[0];
        //            }

        //            string status = UpdateDPData(userid.Trim(), sno.Trim(), newblob);

        //            if (status != "Successful")
        //            {
        //                ret = -1;
        //                msg = ret + "|" + "Operation not successful due to unupdated Token's blob";
        //            }

        //        }


        //        token_activation p = new token_activation()
        //        {

        //            token_serial_number = sno,
        //            user_id = userid,
        //            otp = "",
        //            validation_response = msg,
        //            operation_date = DateTime.Now,
        //            operation = "Online Postactivation",
        //            status = 0
        //        };
        //        context.tokenActivations.Add(p);
        //        context.SaveChanges();

        //        ObjUtil.AcctivityLog(userid,sno,msg,"","");

        //        return ret + "|" + msg;

        //    }
        //    catch (Exception ex)
        //    {
        //        ObjUtil.WriteErrorLog("DERIVATION: " + userid + " - " + sno,
        //                        ex.Message + Environment.NewLine + "************" + Environment.NewLine +
        //                        ex.StackTrace + Environment.NewLine + "***********" + Environment.NewLine + ex.InnerException);


        //        if(ret == 0)
        //        {
        //            ret = -1;
        //        }
        //        return ret + "|" + msg;;

        //    }
        //}

        public string UpdateDPData(string userid, string serialnumber,string dpData)
        {
            string returnMsg = "";
            VacmanContext context = new VacmanContext();
            try
            {



                var clone = _repoDigipass.FindByUserIDAndSno(userid.Trim(), serialnumber.Trim());

                if(clone != null)
                {
                    ObjUtil.WriteErrorLog("before update22222222222222222: " + userid + " -  Bind status" + clone.bind_status,
                             clone.blob + "************" + Environment.NewLine +
                           "Bind Status2222222222222: " + Environment.NewLine + "***********" + "1st");


                    ObjUtil.WriteErrorLog("before update2222222222222: " + userid + " - " + serialnumber,
                           dpData + "************" + Environment.NewLine +
                         "2nd" + Environment.NewLine + "***********" + "2nd");

                    clone.bind_status = 1;
                    clone.blob = dpData;
                    //clone.deviceID = deviceType;
                    _repoDigipass.InsertOrUpdate(clone);
                    _repoDigipass.Save();

                    returnMsg = "Successful";
                }
                else
                {
                    returnMsg = "Unsuccessful";

                    ObjUtil.WriteErrorLog("DERIVATION: " + userid + " - " + serialnumber,
                             "Token not found for the user" + Environment.NewLine + "************" + Environment.NewLine +
                             "Token not found for the user" + Environment.NewLine + "***********" + "Token not found for the user");


                }


            }

            catch (Exception ex)
            {
                ObjUtil.WriteErrorLog("DERIVATION: " + userid + " - " + serialnumber,
                              ex.Message + Environment.NewLine + "************" + Environment.NewLine +
                              ex.StackTrace + Environment.NewLine + "***********" + Environment.NewLine + ex.InnerException);

                returnMsg = "Unsuccessful";
            }

            return returnMsg;
        }

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