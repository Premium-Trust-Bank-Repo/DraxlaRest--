using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data.Entity;
using System.Configuration;
using Domain;
using DataAccessLayer;
using VacmanBoundContext;
using Vasco;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;


namespace VacmanBoundRepo
{


    public class VacmanRepoUOW
    {
        public class DigipassRepo : IdigipassRepoUOW
        {
            VacmanContext context = new VacmanContext();

            //string encrptn_Key = ConfigurationManager.AppSettings["encrptnKey"].ToString(CultureInfo.InvariantCulture);


            public IQueryable<digipass> All
            {

                get { return context.digipasses; }
            }


            public IQueryable<digipass> AllIncluding(params Expression<Func<digipass, object>>[] includingproperties)
            {
                IQueryable<digipass> query = context.digipasses;
                foreach (var includeproperty in includingproperties)
                {
                    query = query.Include(includeproperty);
                }
                return query;
            }


            public void InsertOrUpdate(digipass thedigipass)
            {
                if (thedigipass.id == default(int))
                {
                    //context.UserProfiles.Add(userprofile);
                    context.Entry(thedigipass).State = EntityState.Added;
                }
                else
                {
                    context.Entry(thedigipass).State = EntityState.Modified;
                }
            }

            public digipass Find(int id)
            {
                return context.digipasses.Find(id);
            }

            public digipass FindBySno(string sno)
            {
                var p = (from c in context.digipasses
                         join a in context.uploadedFileDetails
                         on c.batch_no equals a.batch_no
                         where c.serial_no == sno.Trim()
                         select c).FirstOrDefault();

                return p;
            }

            public instance FindBySnoInstance2(string sno, int instanceNo)
            {
                var p = (from c in context.instances
                             //SC_ONL 
                         //where c.serialNo == sno.Trim() && c.mode == "SG" && c.applicationNames.Contains("SC") && c.sequenceNo == instanceNo
                         where c.serial_no == sno.Trim() && c.mode == "SG" && c.application_names.Contains("TDS_HEX") && c.sequence_no == instanceNo && c.status == 0

                         select c).FirstOrDefault();
                return p;
            }

            public instance FindBySnoInstance(string sno)
            {
                var p = (from c in context.instances
                         where c.serial_no == sno.Trim() && c.mode == "SG" && c.application_names.Contains("SC_ACTIV")
                         select c).FirstOrDefault();
                return p;
            }


            //public digipass FindByUserIDAndAuth(string userID, string authcode)
            //{
            //    var p = (from c in context.digipasses
            //             join a in context.uploadedFileDetails
            //             on c.batchNo equals a.batchNo
            //             where c.userId == userID.Trim() && c.AuthCode == authcode.Trim()
            //             select  c).FirstOrDefault();

            //    if(p != null)
            //    {
            //        p.blob = DigipassRepo.DecryptTokenBlobInternal(p.blob);
            //    }

            //    return p;
            //}


            public digipass FindByUserIDAndAuth(string userID, string authcode, string reauthcode)
            {

                ObjUtil.WriteErrorLog("Online Validation",
                      "userID" + userID + Environment.NewLine + "************" + Environment.NewLine +
                      "Authcode" + authcode + Environment.NewLine + "***********" + Environment.NewLine + "online validation resp3 " + "");



                digipass p = null;
                if(authcode != "" && reauthcode == "")
                {
                    p = (from c in context.digipasses
                         where c.user_id.Trim() == userID.Trim() && c.auth_code == authcode.Trim()
                         select c).FirstOrDefault();
                }
                else if(authcode == "" && reauthcode != "")
                {
                    p = (from c in context.digipasses
                         where c.user_id.Trim() == userID.Trim() && c.reauth_code == reauthcode.Trim()
                         select c).FirstOrDefault();
                }
      
                         
                //if (p != null)
                //{
                //    p.blob = DigipassRepo.DecryptTokenBlobInternal(p.blob);
                //}

                return p;
            }

            public digipass FindByUserIDSoft(string userID)
            {


               // ObjUtil.WriteErrorLog("Testing 1", "testing2");
                var p = (from c in context.digipasses
                         where c.user_id == userID.Trim() && c.token_type == "S"
                         select c).FirstOrDefault();

                //ObjUtil.WriteErrorLog("Testing 1.1", "testing 2.2");

                return p;
            }

            public digipass FindByUserID(string userID)
            {
                var p = (from c in context.digipasses
                         join a in context.uploadedFileDetails
                         on c.batch_no equals a.batch_no
                         where c.user_id == userID.Trim() && c.mode == "RO"
                         select c).FirstOrDefault();

                return p;
            }

            public digipass FindByUserIDAndSno(string userID, string sno)
            {
                var p = (from c in context.digipasses
                         //join a in context.uploadedFileDetails
                         //on c.batchNo equals a.batchNo
                         where c.user_id == userID.Trim() && c.mode.Trim() == "RO" && c.serial_no == sno
                         select c).FirstOrDefault();

                return p;
            }

            public digipass FindByNextAvailable(string userid)
            {
                digipass p = null;
        
                p = (from c in context.digipasses
                         where c.token_type == "S" && c.token_state == 0 && c.user_id.Trim() == userid.Trim() 
                         select c).FirstOrDefault();

                if(p == null)
                {
                    p = (from c in context.digipasses
                         where c.token_type == "S" && c.token_state == 0 && c.user_id == null && c.bind_status == 0
                         select c).FirstOrDefault();
                }
                //}

                return p;
            }

            public void Delete(int id)
            {
                var thedigipasses = context.digipasses.Find(id);
                context.digipasses.Remove(thedigipasses);
            }


            public void Save()
            {
                context.SaveChanges();
            }

            public void Dispose()
            {
                context.Dispose();
            }


            public string ValidateOTP(string userId, string blob, string sno, string otp)
            {
                AAL2Wrap oAAL2Wrap;


                try
                {

                    string mythreshold = ConfigurationManager.AppSettings["lockthreshood"].ToString(CultureInfo.InvariantCulture);
                    string itimewindow = ConfigurationManager.AppSettings["iTimeWindow"].ToString(CultureInfo.InvariantCulture);


                    int ret = 0;
                    oAAL2Wrap = new AAL2Wrap
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


                    AAL2Wrap.TKernelParms pg2 = oAAL2Wrap.KParams;

                    int d = pg2.IThreshold;

                    var dp = blob;
                    var temp = blob;


                    var ff = oAAL2Wrap.AAL2GetTokenInfoEx(temp);

                    ret = oAAL2Wrap.AAL2VerifyPassword(ref dp, otp.Trim(), null);

                    dp = string.Empty;
                    var response = oAAL2Wrap.getError(ret); 
                     UpdateDPData(sno, dp);

                     digipass_activities p = new digipass_activities()
                     {
                         user_id = userId,
                         serial_no = sno,
                         operation = "OTP Validation",
                         response = response
                     };
                     context.digipass_activities.Add(p);
                     context.SaveChanges();

                    return response;
                }
                catch (Exception ex)
                {

                    DigipassRepo.ObjUtil.WriteErrorLog("OTP VALIDATION: " + userId + " - " + sno,
                                   ex.Message + Environment.NewLine + "************" + Environment.NewLine +
                                   ex.StackTrace + Environment.NewLine + "***********" + Environment.NewLine + ex.InnerException);

                    return ex.ToString();
                }
            }


            public string ValidateSignature(string dpData, string userid, string sno, string otp, string[] datafield)
            {
                AAL2Wrap oAAL2Wrap;

                string mythreshold = ConfigurationManager.AppSettings["lockthreshood"].ToString(CultureInfo.InvariantCulture);
                string itimewindow = ConfigurationManager.AppSettings["iTimeWindow"].ToString(CultureInfo.InvariantCulture);


                try
                {
                    int ret = 0;
                    oAAL2Wrap = new AAL2Wrap
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


                    AAL2Wrap.TKernelParms pg2 = oAAL2Wrap.KParams;

                    int d = pg2.IThreshold;

                    var dp = dpData;
                    var temp = dpData;


                    var ff = oAAL2Wrap.AAL2GetTokenInfoEx(temp);


                    ret = oAAL2Wrap.AAL2VerifySignature(ref dp, otp, datafield, 3, 0);
                    //ret = oAAL2Wrap.AAL2VerifyPassword(ref dp, otp.Trim(), null);



                    UpdateDPData(sno, dp);


                    dp = string.Empty;
                    var response = oAAL2Wrap.getError(ret); ///(DipassResponse)ret;

                    digipass_activities p = new digipass_activities()
                    {
                        user_id = userid,
                        serial_no = sno,
                        operation = "OTP Validation",
                        response = response
                    };
                    context.digipass_activities.Add(p);
                    context.SaveChanges();

                    return response;
                }
                catch (Exception ex)
                {
                    DigipassRepo.ObjUtil.WriteErrorLog("SIGNATURE VALIDATION: " + userid + " - " + sno,
                                  ex.Message + Environment.NewLine + "************" + Environment.NewLine +
                                  ex.StackTrace + Environment.NewLine + "***********" + Environment.NewLine + ex.InnerException);

                    return ex.ToString();
                }
            }

            private static bool UpdateDPData(string serialnumber, string dpData)
            {
                VacmanContext context = new VacmanContext();
                try
                {
                    var p = (from c in context.digipasses
                             join a in context.uploadedFileDetails
                             on c.batch_no equals a.batch_no
                             where c.serial_no == serialnumber
                             select c).FirstOrDefault();

                    if (p != null)
                    {
                        p.blob = dpData;
                        context.SaveChanges();
                    }
                    return true;
                }

                catch (Exception)
                { return false; }

            }


            public string EncryptTokenBlob(string clearText)
            {
                
                string EncryptionKey = "Password@#21";
               // string EncryptionKey = encrptn_Key;

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

            public  string DecryptTokenBlob(string cipherText)
            {
                //string encrptn_Key = ConfigurationManager.AppSettings["encrptnKey"].ToString(CultureInfo.InvariantCulture);

                string EncryptionKey = "Password@#21";
                //string EncryptionKey = encrptn_Key;

                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }

            public static string DecryptTokenBlobInternal(string cipherText)
            {
                string encrptn_Key = ConfigurationManager.AppSettings["encrptnKey"].ToString(CultureInfo.InvariantCulture);

                string EncryptionKey = encrptn_Key;

                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
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

        public class TokenActivationRepo : ItokenactivationRepoUOW
        {
            VacmanContext context = new VacmanContext();


            public IQueryable<digipass_activities> All
            {

                get { return context.digipass_activities; }
            }


            public IQueryable<digipass_activities> AllIncluding(params Expression<Func<digipass_activities, object>>[] includingproperties)
            {
                IQueryable<digipass_activities> query = context.digipass_activities;
                foreach (var includeproperty in includingproperties)
                {
                    query = query.Include(includeproperty);
                }
                return query;
            }

            public digipass_activities Find(int id)
            {
                return context.digipass_activities.Find(id);
            }


            public  void InsertOrUpdate(digipass_activities thetokenActivations)
            {
                if (thetokenActivations.id == default(int))
                {
                    //context.UserProfiles.Add(userprofile);
                    context.Entry(thetokenActivations).State = EntityState.Added;
                }
                else
                {
                    context.Entry(thetokenActivations).State = EntityState.Modified;
                }
            }

            public void Delete(int id)
            {
                var thetokenActivations = context.digipass_activities.Find(id);
                context.digipass_activities.Remove(thetokenActivations);
            }

            //public void DeleteWithBatchNo(string batchno)
            //{
            //    var theuploadfile = context.uploadedFileDetails.Find(batchno);
            //    context.uploadedFileDetails.Remove(theuploadfile);
            //}


            public void Save()
            {
                context.SaveChanges();
            }

            public void Dispose()
            {
                context.Dispose();
            }

            //public int ActivateToken(string sno, string userid, string blob, string derivationcode)
            //public string ActivateToken(string sno, string userid, string blob, string derivationcode)
            //{
            //    AAL2Wrap oAAL2Wrap;
            //    string is_blob_encrypt = ConfigurationManager.AppSettings["isblobencrypt"].ToString(CultureInfo.InvariantCulture);
            //    VacmanContext context = new VacmanContext();
            //    int ret = 0;
            //    string retBlob = "";
            //    string msg = "";

            //    try
            //    {
            //        //int ret = 0;
            //        oAAL2Wrap = new AAL2Wrap
            //        {
            //            KParams =
            //            {
            //                ParmCount = 19,
            //                ITimeWindow = 30,
            //                STimeWindow = 24,
            //                DiagLevel = 0,
            //                GMTAdjust = 0,
            //                CheckChallenge = 0,
            //                IThreshold = 3,
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
            //        if(is_blob_encrypt == "Yes")
            //        {
            //             actualblob = DigipassRepo.DecryptTokenBlob(blob);

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
                    
            //        //if(is_blob_encrypt == "Yes")
            //        //{
            //        //    retBlob = DigipassRepo.EncryptTokenBlob(dpArray[0]);

            //        //}
            //        //else
            //        //{
            //           // retBlob = dpArray[0];

            //        //}

            //       msg = wrapper.getError(ret);

            //        //UpdateDPData(sno, dpArray[0], userid);

            //        tokenActivation p = new tokenActivation()
            //        {

            //            tokenSerialNumber = sno,
            //            user_id = userid,
            //            Otp = "",
            //            validationResponse = msg,
            //            operationDate = DateTime.Now,
            //            operation = "Online Postactivation",
            //            status = 0
            //        };
            //        context.tokenActivations.Add(p);
            //        context.SaveChanges();

            //        //DigipassRepo.ObjUtil.AcctivityLog("TOKEN ACTIVATION", sno, msg, "", "");

                  
            //        //var p2 = (from f in context.digipasses
            //        //          where f.serialNo == sno && f.mode == "RO" && f.userId == userid
            //        //          select f).FirstOrDefault();

            //        //p2.blob = retBlob;                   
            //        //context.SaveChanges();

            //        //return ret;
            //        return ret.ToString() + "|" + dpArray[0] + "|" + msg;


            //    }
            //    catch (Exception ex)
            //    {
            //        //DigipassRepo.ObjUtil.WriteErrorLog("DERIVATION: " + userid + " - " + sno,
            //        //                ex.Message + Environment.NewLine + "************" + Environment.NewLine +
            //        //                ex.StackTrace + Environment.NewLine + "***********" + Environment.NewLine + ex.InnerException);

            //        return ret.ToString();

            //    }
            //}


        }

        public class TInstanceRepo : IinstanceRepoUOW
        {
            VacmanContext context = new VacmanContext();

            public IQueryable<instance> All
            {

                get { return context.instances; }
            }


            public IQueryable<instance> AllIncluding(params Expression<Func<instance, object>>[] includingproperties)
            {
                IQueryable<instance> query = context.instances;

                foreach (var includeproperty in includingproperties)
                {
                    query = query.Include(includeproperty);
                }
                return query;
            }

            public instance Find(int id)
            {
                return context.instances.Find(id);
            }


            public void InsertOrUpdate(instance thetinstance)
            {
                context.Entry(thetinstance).State = EntityState.Added;
            }

            public void UpdateOnly(instance thetinstance)
            {
               context.Entry(thetinstance).State = EntityState.Modified;
            }

            public void DeleteInstances(string sno, int instaceno)
            {
                // var list = context.instances.Where(c => c.serial_no == sno && c.sequence_no ==instaceno).ToList();
                var list = context.instances.Where(c => c.serial_no == sno).ToList();
                context.instances.RemoveRange(list);
                context.SaveChanges();
            }

            public void DeleteInstancesBySno(string sno)
            {
                var list = context.instances.Where(c => c.serial_no == sno).ToList();
               // var list = context.instances.Where(c => c.serial_no == sno);
                context.instances.RemoveRange(list);
                context.SaveChanges();
            }







            public void Delete(int id)
            {
                var thetinstance = context.instances.Find(id);
                context.instances.Remove(thetinstance);
            }

            public void Save()
            {
                context.SaveChanges();
            }

            public void Dispose()
            {
                context.Dispose();
            }
        }



    }
    public interface IdigipassRepoUOW : IEntityRepository<digipass>
        {

        }

        public interface ItokenactivationRepoUOW : IEntityRepository<digipass_activities>
        {

        }
        public interface IinstanceRepoUOW : IEntityRepository<instance>
        {
            
        }

    public interface IdigipassUserRepoUOW : IEntityRepository<digipass_users>
    {

    }
}
