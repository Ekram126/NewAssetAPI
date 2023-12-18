using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Asset.Models;
using Asset.ViewModels.UserVM;
using System.Net;
using System.Net.Sockets;
using Asset.API.Helpers;
using Asset.Domain;
using System.Net.Mail;
using Asset.Domain.Services;

namespace Asset.API.Controllers.MobileController
{
    [Route("mobile/api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly ISettingService _settingService;

        string strInsitute, strInsituteAr, strLogo = "";
        bool isAgency, isScrap, isVisit, isExternalFix, isOpenRequest, canAdd;

        public LoginController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IEmailSender emailSender, IConfiguration configuration, ISettingService settingService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _settingService = settingService;
            _context = context;
        }




        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginVM userObj)
        {
            string Useremail = "";
            string userName = "";
            string userNameAr = "";
            var user = await _userManager.FindByNameAsync(userObj.Username);

            if (user == null)
            {
                if (userObj.Lang == "ar")
                    return Ok(new { data = "", msg = "لا يوجد مستخدم بهذا الاسم", status = "0" });
                if (userObj.Lang == "en")
                    return Ok(new { data = "", msg = "User does not exists!", status = "0" });
              else
                    return Ok(new { data = "", msg = "User does not exists!", status = "0" });

            }
            // return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User does not exists!", MessageAr = "لا يوجد مستخدم بهذا الاسم" });


            var userpass = await _userManager.CheckPasswordAsync(user, userObj.PasswordHash);
            if (userpass == false)
            {
                if (userObj.Lang == "ar")
                    return Ok(new { data = "", msg = "كلمة المرور غير صحيحة", status = "0" });
                if (userObj.Lang == "en")
                    return Ok(new { data = "", msg = "Invalid Password!", status = "0" });
                else
                    return Ok(new { data = "", msg = "Invalid Password!", status = "0" });
            }
            
            if (user != null && await _userManager.CheckPasswordAsync(user, userObj.PasswordHash))
            {

                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                foreach (var claim in authClaims)
                {
                    await _userManager.AddClaimAsync(user, claim);

                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );


                var id = user.Id;
                var name = user.UserName;
                var governorateId = user.GovernorateId;
                var cityId = user.CityId;
                var hospitalId = user.HospitalId;
                var supplierId = user.SupplierId;
                var commetieeMemberId = user.CommetieeMemberId;

                var govName = user.GovernorateId > 0 ? _context.Governorates.Where(a => a.Id == user.GovernorateId).First().Name : "";
                var cityName = user.CityId > 0 ? _context.Cities.Where(a => a.Id == user.CityId).First().Name : "";
                var govNameAr = user.GovernorateId > 0 ? _context.Governorates.Where(a => a.Id == user.GovernorateId).First().NameAr : "";
                var cityNameAr = user.CityId > 0 ? _context.Cities.Where(a => a.Id == user.CityId).First().NameAr : "";
                var hospitalName = user.HospitalId > 0 ? _context.Hospitals.Where(a => a.Id == user.HospitalId).First().Name : "";

                List<string> userRoleNames = new List<string>();
                var hospitalNameAr = user.HospitalId > 0 ? _context.Hospitals.Where(a => a.Id == user.HospitalId).First().NameAr : "";
                var hospitalCode = user.HospitalId > 0 ? _context.Hospitals.Where(a => a.Id == user.HospitalId).First().Code : "";
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _roleManager.Roles on userRole.RoleId equals role.Id
                                 where user.Id == userRole.UserId
                                 select role);
                foreach (var item in roleNames)
                {
                    userRoleNames.Add(item.Name);
                }

                if (supplierId > 0)
                {
                    var lstSuppliers = _context.Suppliers.Where(a => a.EMail == user.Email).ToList();
                    if (lstSuppliers.Count > 0)
                    {
                        var supplierObj = lstSuppliers[0];
                        Useremail = user.Email;
                        userName = supplierObj.Name;
                        userNameAr = supplierObj.NameAr;
                    }
                }
                if (commetieeMemberId > 0)
                {
                    var lstMembers = _context.CommetieeMembers.Where(a => a.EMail == user.Email).ToList();
                    if (lstMembers.Count > 0)
                    {
                        var memberObj = lstMembers[0];
                        Useremail = user.Email;
                        userName = memberObj.Name;
                        userNameAr = memberObj.NameAr;
                    }
                }
                else
                {
                    Useremail = user.Email;
                    userName = user.UserName;
                }

                var lstSettings = _settingService.GetAll().ToList();
                if (lstSettings.Count > 0)
                {
                    foreach (var item in lstSettings)
                    {
                        if (item.KeyName == "Institute")
                        {
                            strInsitute = item.KeyValue;
                            strInsituteAr = item.KeyValueAr;
                        }
                        if (item.KeyName == "Logo")
                            strLogo = item.KeyValue;


                        if (item.KeyName == "PMAgency")
                            isAgency = Convert.ToBoolean(item.KeyValue);


                        //if (item.KeyName == "IsVisit")
                        //    isVisit = Convert.ToBoolean(item.KeyValue);

                        if (item.KeyName == "IsOpenRequest")
                            isOpenRequest = Convert.ToBoolean(item.KeyValue);
                    
                    }
                }


                var userDataObj = new 
                {
                    UserName = userName,
                    Id = id,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RoleNames = userRoleNames,
                    GovernorateId = governorateId,
                    CityId = cityId,
                    HospitalId = hospitalId,
                    SupplierId = supplierId,
                    CommetieeMemberId = commetieeMemberId,
                    GovernorateName = govName,
                    CityName = cityName,
                    HospitalName = hospitalName,
                    GovernorateNameAr = govNameAr,
                    CityNameAr = cityNameAr,
                    HospitalNameAr = hospitalNameAr,
                    HospitalCode = hospitalCode,
                    strInsitute = strInsitute,
                    strInsituteAr = strInsituteAr,
                    strLogo = strLogo,
                    isAgency = isAgency,
                    isScrap = isScrap,
                    isVisit = isVisit,
                    isOpenRequest = isOpenRequest,
                    isExternalFix = isExternalFix,
                    canAdd= canAdd
                };



                return Ok( new { data = userDataObj, msg = "Success", status = '1' } );
            }
            return Unauthorized();
        }




    }
}
