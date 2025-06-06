using Microsoft.EntityFrameworkCore;
using swas.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.DAL.Models
{
    // Created by Sub Maj Sanal on 13 Nov 23 for blog attachment
    //public class AttHistComment
    //{
    //    [Key]
    //    public int Attid { get; set; }

    //    [ForeignKey("ProjStakeHolderMov")]
    //    public int PsmId { get; set; }
    //    public string AttPath { get; set; }

    //    [ForeignKey("StkComment")]
    //    public int StkCommentId { get; set; }
    //    public DateTime? TimeStamp { get; set; }
    //    public bool IsDeleted { get; set; }
    //    public bool IsActive { get; set; }
    //    public string? EditDeleteBy { get; set; }
    //    public DateTime EditDeleteDate { get; set; }
    //    public string? UpdatedByUserId { get; set; }
    //    public DateTime? DateTimeOfUpdate { get; set; }
    //    [StringLength(1500)]
    //    public string? Reamarks { get; set; }
    //    public string ActFileName { get; set; }

       
    //}
}
// stkcomment





//< td colspan = "2" style = "color:black;" >
//                                                  @Html.HiddenFor(model => model.DataProjId, new { htmlAttributes = new { @class = "form-control" } })


//                                                          @Html.HiddenFor(model => model.ProjMov.PsmId, new { htmlAttributes = new { @class = "form-control", id = "PsmIds" } })
//                                                          @Html.HiddenFor(model => model.Submitcde, new { htmlAttributes = new { @class = "form-control" } })
//                                                          @Html.HiddenFor(model => model.ProjMov.ProjCode, new { htmlAttributes = new { @class = "form-control" } })
//                                                          Attach Document Description
//                                                      </td>


//                                          <td colspan="2">


//                                                  @Html.TextAreaFor(model => model.ProjMov.AttRemarks, new { htmlAttributes = new { @class = "form-control", oninput = "ValInData(this)" } })


//                                              </ td >
//                                          </ tr > < tr >
//                                                      < td colspan = "4" >
//                                                          < center >
//                                                              Select PDF file
//                                                              @*<input type="file" id="pdfInput" multiple>*@

//                                                              <input style="color: black;" type="file" id="pdfFileInput" name="uploadfile" class= "form-control-file" accept = ".pdf" >
//                                                          </ center >
//                                                      </ td >

//                                                  </ tr >






//public async Task<IActionResult> FwdCreate(Projmove projStakeHolderMov, IFormFile uploadfile)
//{

//    string webRootPath = _environment.WebRootPath;
//    //string fileName = "";
//    //string virtualPath = "";

//    string scheme = _httpContextAccessor.HttpContext.Request.Scheme;
//    string host = _httpContextAccessor.HttpContext.Request.Host.Value;
//    string fullUrl = $"{scheme}://{host}/Uploads/";
//    //var uploadsFolderPath = Path.Combine(fullUrl);

//    try
//    {


//        tbl_Projects Tbproj = new tbl_Projects();
//        Tbproj = await _projectsRepository.GetProjectByIdAsync(projStakeHolderMov.DataProjId ?? 0);
//        tbl_Projects pjt = new tbl_Projects();
//        pjt = await _projectsRepository.GetProjectByIdAsync(projStakeHolderMov.DataProjId ?? 0);
//        ProjIDRes PjIR = new ProjIDRes();
//        if (projStakeHolderMov.Submitcde != true)
//        {
//            List<ProjHistory> projHist = await _projectsRepository.GetProjectHistorybyID(projStakeHolderMov.DataProjId);

//            tbl_AttHistory atthis = new tbl_AttHistory();
//            if (uploadfile != null && uploadfile.Length > 0)
//            {
//                string uniqueFileName = $"{"Swas"}_{Guid.NewGuid()}{Path.GetExtension(uploadfile.FileName)}";

//                string filePath = Path.Combine("wwwroot/Uploads/", uniqueFileName);


//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    uploadfile.CopyTo(stream);
//                }

//                projStakeHolderMov.Atthistory[0].AttPath = uniqueFileName;
//            }


//            // projStakeHolderMov.Atthistory[0].AttPath = atthis.AttPath;
//            if (uploadfile.FileName != null && projStakeHolderMov.Atthistory[0].Reamarks != null)
//            {
//                projStakeHolderMov.Atthistory[0].Reamarks = projStakeHolderMov.ProjMov.AttRemarks;
//                projStakeHolderMov.Atthistory[0].ActFileName = uploadfile.FileName;
//            }

//            //start
//            int psmid = await _psmRepo.AddProjStakeHolderMovAsync(projStakeHolderMov);

//            PjIR = swas.BAL.Utility.ExtensionMethods.FirstSecond(pjt.ProjName, projStakeHolderMov.DataProjId ?? 0, psmid);

//            if (Tbproj.ProjCode == null)
//            {
//                Tbproj.ProjCode = PjIR.PorjPin;
//                _projectsRepository.UpdateProjectAsync(Tbproj);
//                ViewBag.PjIR = PjIR;
//            }
//            else
//            {
//                ViewBag.PjIR = Tbproj.ProjCode;
//            }


//            MyRequestModel mrmodel = new MyRequestModel();
//            mrmodel.DtaProjID = projStakeHolderMov.DataProjId ?? 0;
//            mrmodel.AttPath = atthis.AttPath;
//            mrmodel.AttDocuDesc = atthis.Reamarks;
//            mrmodel.ActFileName = uploadfile.FileName;
//            mrmodel.PsmId = psmid;
//            mrmodel.Projpin = Tbproj.ProjCode;

//            var serializedData = JsonConvert.SerializeObject(mrmodel);
//            string ProtectedValue = _dataProtector.Protect(serializedData);
//            EncryModel encryModel = new EncryModel();
//            encryModel.EncryItem = ProtectedValue;


//            ViewBag.PsmId = psmid;
//            ViewBag.DataProjId = projStakeHolderMov.DataProjId;

//            // TempData["SuccessMessage"] = "Please note down the Proj Code for Further Enquiries.. (Fwd & Ready for Attach more Docu)...";

//            // end

//            return RedirectToAction("ProjHistory", "Projects", encryModel);
//            // 1 time
//            // Process the form data
//            ViewBag.SubmitCde = true; // for subsequent submissions
//        }
//        else
//        {
//            Login Logins = SessionHelper.GetObjectFromJson<Login>(_httpContextAccessor.HttpContext.Session, "User");
//            // 2nd time
//            // Handle
//            // 
//            ViewBag.PjIR = projStakeHolderMov.ProjMov.ProjCode;
//            ViewBag.PsmId = projStakeHolderMov.ProjMov.PsmId;
//            tbl_AttHistory atthis = new tbl_AttHistory();
//            if (uploadfile != null && uploadfile.Length > 0)
//            {
//                string uniqueFileName = $"{"Swas"}_{Guid.NewGuid()}{Path.GetExtension(uploadfile.FileName)}";
//                //string filePath = Path.Combine(fullUrl, uniqueFileName);
//                string filePath = Path.Combine("wwwroot/Uploads/", uniqueFileName);


//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    uploadfile.CopyTo(stream);
//                }

//                atthis.AttPath = uniqueFileName;
//                atthis.Reamarks = projStakeHolderMov.ProjMov.AttRemarks;
//                atthis.ActFileName = uploadfile.FileName;
//            }
//            ViewBag.SubmitCde = true;


//            MyRequestModel mrmodel = new MyRequestModel();
//            mrmodel.DtaProjID = projStakeHolderMov.DataProjId ?? 0;
//            mrmodel.AttPath = atthis.AttPath;
//            mrmodel.AttDocuDesc = atthis.Reamarks;
//            mrmodel.ActFileName = uploadfile.FileName;
//            mrmodel.PsmId = projStakeHolderMov.ProjMov.PsmId;
//            var serializedData = JsonConvert.SerializeObject(mrmodel);
//            string ProtectedValue = _dataProtector.Protect(serializedData);
//            EncryModel encryModel = new EncryModel();
//            encryModel.EncryItem = ProtectedValue;


//            tbl_AttHistory atthist = new tbl_AttHistory();
//            atthist.AttPath = atthis.AttPath;
//            atthist.Reamarks = atthis.Reamarks;
//            atthist.ActFileName = uploadfile.FileName;
//            atthist.PsmId = projStakeHolderMov.ProjMov.PsmId;
//            atthist.UpdatedByUserId = Logins.ActualUserName + "(" + Logins.Unit + ")";
//            atthist.DateTimeOfUpdate = DateTime.Now;
//            atthist.IsDeleted = false;
//            atthist.IsActive = true;
//            atthist.EditDeleteBy = Logins.ActualUserName + "(" + Logins.Unit + ")";
//            atthist.ActionId = projStakeHolderMov.ProjMov.ActionId;
//            atthist.TimeStamp = DateTime.Now;
//            atthist.EditDeleteDate = DateTime.Now;

//            _attHistoryRepository.AddAttHistoryAsync(atthist);
//            TempData["SuccessMessage"] = "Docu Attached...";
//            return RedirectToAction("ProjHistory", "Projects", encryModel);
//        }

//    }
//    catch (Exception ex)
//    {
//        tbl_AttHistory atthist = new tbl_AttHistory();
//        atthist.AttPath = ex.Message;
//        atthist.ActFileName = ex.Message;
//        atthist.PsmId = 88;
//        atthist.UpdatedByUserId = "system";
//        atthist.DateTimeOfUpdate = DateTime.Now;
//        atthist.IsDeleted = false;
//        atthist.IsActive = true;
//        atthist.EditDeleteBy = "System";
//        atthist.ActionId = projStakeHolderMov.ProjMov.ActionId;
//        atthist.TimeStamp = DateTime.Now;
//        atthist.EditDeleteDate = DateTime.Now;

//        Response.WriteAsJsonAsync(ex.Message.ToString());
//        _attHistoryRepository.AddAttHistoryAsync(atthist);
//        Console.WriteLine(ex.Message);
//        return RedirectToAction("ProjHistory", "Projects");
//    }


//    // return View("ProjHistory", projHist);
//}
