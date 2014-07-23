using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Model;
using System.Configuration;
using DAL.Interface;
using System.Data;
using DAL.Helper;

namespace BLL
{
    /// <summary>
    /// 用户相关业务
    /// </summary>
    public class UserBLL
    {
        /// <summary>
        /// 普通用户登录
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="uPassword"></param>
        /// <param name="user"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int UserSignin(string userCode, string uPassword,out User user,out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append("SELECT * FROM dbo.tUser WHERE UserCode=@UserCode AND UPassword=@UPassword AND Enabled=1");
                    dal.OpenReader(sql.ToString(),
                       dal.CreateParameter("@UserCode",userCode),
                       dal.CreateParameter("@UPassword", Des.EncryStrHex(uPassword,userCode))
                        );
                    if (dal.DataReader.Read())
                    {
                        //登录成功
                        user = new User()
                        {
                            ID = Convert.ToDecimal(dal.DataReader["ID"]),
                            UserCode = Convert.ToString(dal.DataReader["UserCode"]).TrimEnd(),
                            UserName = Convert.ToString(dal.DataReader["UserName"]).TrimEnd(),
                            IsAdmin = Convert.ToDecimal(dal.DataReader["IsAdmin"]) == 1 ? true : false
                        };
                        msg = "登录成功";
                    }
                    else
                    {
                        msg = "用户名或密码错误";
                        user = null;
                        //登录失败
                    }
                }
                return user == null ? 0 : 1;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                user = null;
                return -1;
            }
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="uPassword"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int AdminSignin(string userCode, string uPassword,out User user, out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append("select count(*) userCount from tUser where IsAdmin=1 And Enabled=1");
                    dal.OpenReader(sql.ToString());
                    int i=-1;
                    if (dal.DataReader.Read())
                    {
                        i = Convert.ToInt32(dal.DataReader["userCount"]);
                    }
                    dal.DataReader.Close();
                    if (i == 0)
                    {
                        //梅伊欧用户
                        if (userCode == "admin" && uPassword == "753951")
                        {
                            msg = "默认用户登录成功，请尽快添加管理员账户";
                            //登录成功
                            user = new User()
                            {
                                ID = 0,
                                UserCode = "admin",
                                UserName = "默认管理员",
                                IsAdmin = true
                            };
                            return 1;
                        }
                        else
                        {
                            msg = "用户名或密码错误";
                            user = null;
                            return 0;
                        }
                    }
                    else
                    {
                        sql.Clear();
                        sql.Append("SELECT * FROM tUser WHERE UserCode=@UserCode AND UPassword=@UPassword AND IsAdmin='1' AND Enabled=1 ");
                        dal.OpenReader(sql.ToString(),
                           dal.CreateParameter("@UserCode", userCode),
                           dal.CreateParameter("@UPassword", Des.EncryStrHex(uPassword, userCode))
                            );
                        if (dal.DataReader.Read())
                        {

                            //登录成功
                            msg = "登录成功";
                            //登录成功
                            user = new User()
                            {
                                ID = Convert.ToDecimal(dal.DataReader["ID"]),
                                UserCode = Convert.ToString(dal.DataReader["UserCode"]).TrimEnd(),
                                UserName = Convert.ToString(dal.DataReader["UserName"]).TrimEnd(),
                                IsAdmin = Convert.ToDecimal(dal.DataReader["IsAdmin"]) == 1 ? true : false
                            };
                            return 1;
                        }
                        else
                        {
                            msg = "用户名或密码错误";
                            user = null;
                            return 0;
                        }
                    }
                    
                    
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                user = null;
                return -1;
            }
        }

        /// <summary>
        /// 列表用户
        /// </summary>
        /// <returns></returns>
        public static ICollection<User> ListUser()
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    dal.OpenReader("SELECT ID,UserCode,UserName,IsAdmin,Enabled FROM tUser");
                    ICollection<User> users = ObjectHelper.BuildObject<User>(dal.DataReader);
                    return users;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 增加用户
        /// </summary>
        /// <returns></returns>
        public static int AddUser(User user,out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append("insert into tUser(UserCode,UserName,UPassword,IsAdmin,Enabled) ");
                    sql.Append("Values (");
                    sql.AppendFormat("@UserCode,@UserName,@UPassword,@IsAdmin,@Enabled");
                    sql.Append(")");
                    dal.BeginTran();
                    int i;
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@UserCode", user.UserCode),
                        dal.CreateParameter("@UserName", user.UserName),
                        dal.CreateParameter("@UPassword", Des.EncryStrHex(user.UserCode, user.UserCode)),
                        dal.CreateParameter("@IsAdmin", user.IsAdmin ? 1 : 0),
                        dal.CreateParameter("@Enabled",user.Enabled)
                        );
                    if (i ==1)
                    {
                        dal.CommitTran();
                        msg = "success";
                        return i;
                    }
                    else
                    {
                        msg = "error";
                        dal.RollBackTran();
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int UpdateUser(User user, out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append("update tUser set UserName=@UserName,IsAdmin=@IsAdmin ,Enabled=@Enabled where ID=@ID");
                    dal.BeginTran();
                    int i;
                    dal.Execute(sql.ToString(),out i,
                        dal.CreateParameter("@UserName",user.UserName),
                        dal.CreateParameter("@IsAdmin",user.IsAdmin?1:0),
                        dal.CreateParameter("@Enabled",user.Enabled),
                        dal.CreateParameter("@ID",user.ID)
                        );
                    if (i == 1)
                    {
                        dal.CommitTran();
                        msg = "success";
                        return i;
                    }
                    else
                    {
                        dal.RollBackTran();
                        msg = "error";
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int DeleteUser(decimal ID, out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append("Delete from tUser where ID=@ID");
                    dal.BeginTran();
                    int i;
                    dal.Execute(sql.ToString(), out i, dal.CreateParameter("@ID", ID));
                    if (i == 1)
                    {
                        dal.CommitTran();
                        msg = "success";
                        return i;
                    }
                    else
                    {
                        dal.RollBackTran();
                        msg = "error";
                        return 0;
                    }
                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="password"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int ChangePwd(decimal ID, string oldPwd,string newPwd, out string msg)
        {
            try
            {
                using (IDAL dal = DALBuilder.CreateDAL(ConfigurationManager.ConnectionStrings["SYSDB"].ConnectionString, 0))
                {
                    dal.BeginTran();
                    StringBuilder sql = new StringBuilder(256);
                    sql.Append("SELECT UPassword ,UserCode FROM tUser WHERE ID=@ID ");
                    dal.OpenReader(sql.ToString(),
                        dal.CreateParameter("@ID", ID)
                        );
                    string oldDbPwd,UserCode;
                    if (dal.DataReader.Read())
                    {
                        oldDbPwd = Convert.ToString(dal.DataReader["UPassword"]).TrimEnd();
                        UserCode=Convert.ToString(dal.DataReader["UserCode"]).TrimEnd();
                    }
                    else
                    {
                        msg = "未找到该用户";
                        return 0;
                    }
                    dal.DataReader.Close();
                    if (!oldDbPwd.Equals(Des.EncryStrHex(oldPwd,UserCode)))
                    {
                        //密码验证失败
                        msg = "密码验证失败";
                        return 0;
                    }
                    sql.Clear();
                    sql.Append("UPDATE tUser SET UPassword=@UPassword WHERE ID=@ID");
                    int i;
                    dal.Execute(sql.ToString(), out i,
                        dal.CreateParameter("@UPassword", Des.EncryStrHex(newPwd, UserCode)),
                        dal.CreateParameter("@ID", ID)
                        );
                    if (i == 1)
                    {
                        dal.CommitTran();
                        msg = "success";
                        return 1;
                    }
                    else
                    {
                        dal.RollBackTran();
                        msg = "error";
                        return 0;
                    }

                }
            }
            catch (System.Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }
    }
}
