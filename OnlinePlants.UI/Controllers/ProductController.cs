using Dapper;
using MazzGlobal.Data.Common;
using MazzGlobal.UI.Models;
using OnlinePlants.Data;
using OnlinePlants.Model.BusinessModel;
using OnlinePlants.UI.CommonFunction;
using OnlinePlants.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MazzGlobal.UI.Controllers
{
    public class ProductController : Controller
    {
        OnlinePlantsContext context = new OnlinePlantsContext();
        private readonly IRepository<Category> _CategoryRepository;
        private readonly IRepository<MetaKeyword> _MetaKeywordRepository;
        private readonly IRepository<Product> _ProductRepository;
        private readonly IRepository<ProductPicture> _ProductPictureRepository;

        public ProductController()
        {
            _CategoryRepository = new Repository<Category>(context);
            _MetaKeywordRepository = new Repository<MetaKeyword>(context);
            _ProductRepository = new Repository<Product>(context);
            _ProductPictureRepository = new Repository<ProductPicture>(context);
        }

        public ActionResult Product(string category, string subCategory)
        {
            CategoryModel model = new CategoryModel();
            try
            {
                string subCatName = CommonFunction.GetCategoryName(subCategory);
                string mainCatName = CommonFunction.GetCategoryName(category);
                if (subCategory != null)
                {
                    var categoryDetail = _CategoryRepository.SearchFor(x => x.Name == mainCatName && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                    var subCategoryDetail = _CategoryRepository.SearchFor(x => x.Name == subCatName && x.ParentId == categoryDetail.ID && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                    if (subCategoryDetail != null)
                    {
                        model.ID = subCategoryDetail.ID;
                        model.Name = subCategoryDetail.Name;
                        model.Description = subCategoryDetail.Description;
                    }
                }
                //else
                //{
                //    var categoryDetail = _CategoryRepository.SearchFor(x => x.Name == name && x.IsActive == true && x.IsDelete == false).FirstOrDefault();
                //    if (categoryDetail != null)
                //    {
                //        model.CategoryID = categoryDetail.ID;
                //        model.CategoryName = categoryDetail.Name;
                //        model.Description = categoryDetail.ShortDescription;
                //        model.FullDescription = categoryDetail.Description;
                //        model.BannerContent = categoryDetail.BannerContent;
                //        model.MainCateName = name;
                //        model.MainCategoryId = categoryDetail.ID;
                //        model.MainCatUrl = "/products/" + category + "/" + mainCategory;
                //        ViewBag.CatType = 1;
                //        model.BradcumURL = "<ol class='breadcrumb'><li><a href='/'>Home</a></li><li>" + RootCat + "</li><li class='active'>" + model.CategoryName + "</li></ol>";
                //    }
                //}

                var pageName = CommonFunction.GetPageName(subCategory);
                var metaContent = _MetaKeywordRepository.SearchFor(c => c.PageName == pageName).FirstOrDefault();
                if (metaContent != null)
                {
                    ViewBag.metatitle = metaContent == null ? "NaturesBuggy" : metaContent.Title;
                    ViewBag.metaContent = metaContent == null ? "NaturesBuggy" : metaContent.Description;
                    ViewBag.metakeyword = metaContent == null ? "NaturesBuggy" : metaContent.Keywords;
                    ViewBag.OgImage = metaContent == null ? "NaturesBuggy" : Request.Url + "Content/OgImage/" + metaContent.OgImageUrl;
                    ViewBag.Url = Request.Url;
                }
                else
                {
                    ViewBag.metatitle = "Buy " + subCategory + " online in india";
                    ViewBag.metaContent = "Shop " + subCategory + " at lowest price online in India. NaturesBuggy offer 100% genuine & best quality healthcare Products in India";
                    ViewBag.metakeyword = _MetaKeywordRepository.SearchFor(c => c.PageName == subCategory).FirstOrDefault() != null ? _MetaKeywordRepository.SearchFor(c => c.PageName == subCategory).FirstOrDefault().Keywords : "NaturesBuggy";
                    ViewBag.OgImage = metaContent == null ? "NaturesBuggy" : metaContent.OgImageUrl;
                    ViewBag.Url = Request.Url;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ErrorLog(ex);
            }
            return View(model);
        }

        public ActionResult ProductDetailPage(string subCategory, int productId)
        {
            ProductDetailModel model = new ProductDetailModel();
            var productDetail = _ProductRepository.GetById(productId);
            if (productDetail != null)
            {
                try
                {                 
                    using (var sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["OnlinePlants"].ConnectionString))
                    {
                        model = sqlConnection.Query<ProductDetailModel>("exec GetProductDetailById @productID, @attributeID", new { productID = productId }).FirstOrDefault();
                        ViewBag.metatitle = "Buy Online " + model.Name + " from Natures Buggy.";
                        ViewBag.metaContent = "Shop " + model.Name + " at lowest price online in India. NaturesBuggy offer 100% genuine & best quality healthcare Products in India";
                        ViewBag.OgImage = model.ProductPicture;
                        ViewBag.Url = Request.Url;
                    }           
                }
                catch (Exception ex)
                {
                    CommonFunction.ErrorLog(ex);
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("404Page", "Home");
            }
        }

    }
}