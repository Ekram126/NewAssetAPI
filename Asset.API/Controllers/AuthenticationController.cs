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

namespace Asset.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly ISettingService _settingService;


        string strInsitute, strInsituteAr, strLogo = "";
        bool isAgency, isScrap, isVisit, isExternalFix, isOpenRequest, canAdd;
        int hospitalTypeNum = 0;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IEmailSender emailSender, IConfiguration configuration, ISettingService settingService, ApplicationDbContext context)
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
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User does not exists!" });

            var userpass = await _userManager.CheckPasswordAsync(user, userObj.PasswordHash);
            if (userpass == false)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Invalid Password!" });


            if (user != null && await _userManager.CheckPasswordAsync(user, userObj.PasswordHash))
            {


                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                   // new Claim("key","value"),
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
                    //      expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );


                var id = user.Id;
                var name = user.UserName;
                var governorateId = user.GovernorateId;
                var cityId = user.CityId;
                var orgId = user.OrganizationId;
                var subOrgId = user.SubOrganizationId;
                var hospitalId = user.HospitalId;
                var supplierId = user.SupplierId;
                var commetieeMemberId = user.CommetieeMemberId;

                var govName = user.GovernorateId > 0 ? _context.Governorates.Where(a => a.Id == user.GovernorateId).First().Name : "";
                var cityName = user.CityId > 0 ? _context.Cities.Where(a => a.Id == user.CityId).First().Name : "";
                var orgName = user.OrganizationId > 0 ? _context.Organizations.Where(a => a.Id == user.OrganizationId).First().Name : "";
                var subOrgName = user.SubOrganizationId > 0 ? _context.SubOrganizations.Where(a => a.Id == user.SubOrganizationId).First().Name : "";
                var govNameAr = user.GovernorateId > 0 ? _context.Governorates.Where(a => a.Id == user.GovernorateId).First().NameAr : "";
                var cityNameAr = user.CityId > 0 ? _context.Cities.Where(a => a.Id == user.CityId).First().NameAr : "";
                var orgNameAr = user.OrganizationId > 0 ? _context.Organizations.Where(a => a.Id == user.OrganizationId).First().NameAr : "";
                var subOrgNameAr = user.SubOrganizationId > 0 ? _context.SubOrganizations.Where(a => a.Id == user.SubOrganizationId).First().NameAr : "";


                var hospitalName = user.HospitalId > 0 ? _context.Hospitals.Where(a => a.Id == user.HospitalId).First().Name : "";



                var hospitalNameAr = user.HospitalId > 0 ? _context.Hospitals.Where(a => a.Id == user.HospitalId).First().NameAr : "";
                var hospitalCode = user.HospitalId > 0 ? _context.Hospitals.Where(a => a.Id == user.HospitalId).First().Code : "";
                var roleNames = (from userRole in _context.UserRoles
                                 join role in _roleManager.Roles on userRole.RoleId equals role.Id
                                 where user.Id == userRole.UserId
                                 select role);

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

                      

                        if (item.KeyName == "IsOpenRequest")
                            isOpenRequest = Convert.ToBoolean(item.KeyValue);

            
                        if (item.KeyName == "HospitalType")
                            hospitalTypeNum = Convert.ToInt32(item.KeyValue);

                    }
                }

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    email = Useremail,
                    userName = userName,
                    userNameAr = userNameAr,
                    roles = userRoles,
                    expiration = token.ValidTo,
                    id = id,
                    roleNames = roleNames,
                    governorateId = governorateId,
                    cityId = cityId,
                    organizationId = orgId,
                    subOrganizationId = subOrgId,
                    hospitalId = hospitalId,
                    supplierId = supplierId,
                    commetieeMemberId = commetieeMemberId,
                    govName = govName,
                    cityName = cityName,
                    orgName = orgName,
                    subOrgName = subOrgName,
                    hospitalName = hospitalName,
                    govNameAr = govNameAr,
                    cityNameAr = cityNameAr,
                    orgNameAr = orgNameAr,
                    subOrgNameAr = subOrgNameAr,
                    hospitalNameAr = hospitalNameAr,
                    hospitalCode = hospitalCode,
                    strInsitute = strInsitute,
                    strInsituteAr = strInsituteAr,
                    strLogo = strLogo,
                    isAgency = isAgency,
                    isScrap = isScrap,
                    isVisit = isVisit,
                    isOpenRequest = isOpenRequest,
                    isExternalFix = isExternalFix,
                    canAdd = canAdd,
                    hospitalTypeNum = hospitalTypeNum
                });
            }
            return Unauthorized();
        }




        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordVM forgotPasswordModel)
        {
            string replace = "";
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user == null)
                return BadRequest("Invalid Request");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
             {
                 {"email", forgotPasswordModel.Email },
                 {"token", token }

             };

            var callback = QueryHelpers.AddQueryString(forgotPasswordModel.ClientURI, param);
            var hash = callback.Split("#");
            var query = hash[0];
            replace = query.Replace("/?", "/#/reset?");


            // replace = query.Replace("ResetPassword?", "#/ResetPassword?");


            StringBuilder strBuild = new StringBuilder();
            strBuild.Append("Dear " + user.UserName + ":");
            strBuild.Append("<br />");
            strBuild.Append("من فضلك اضغط على الرابط التالي لتغيير كلمة المرور");
            strBuild.Append("<br />");
            strBuild.Append("<a href='" + replace + "'>اضغط هنا</a>");


            string from = "almostakbaltechnology.dev@gmail.com";
            string subject = "Al-Mostakbal Technology";
            string body = strBuild.ToString();
            string appSpecificPassword = "fajtjigwpcnxyyuv";


            var mailMessage = new MailMessage(from, user.Email, subject, body);
            mailMessage.IsBodyHtml = true;
            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(from, appSpecificPassword);
                smtpClient.Send(mailMessage);
            }





            return Ok();
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordVM resetPasswordDto)
        {
            List<string> lstErrors = new List<string>();
            string errormessage = "";
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetPassResult = await _userManager.ResetPasswordAsync(user, code, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);

                foreach (var error in errors)
                {
                    lstErrors.Add(error);

                }
                errormessage = string.Join("<br />", lstErrors);
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = errormessage, MessageAr = errormessage });
            }

            return Ok(lstErrors);
        }




    }
}
