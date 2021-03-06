﻿using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using LearningSystem.Data;
using LearningSystem.Models.EntityModels;
using LearningSystem.Models.ViewModels.Course;
using LearningSystem.Models.ViewModels.Filtering;
using LearningSystem.Services;
using LearningSystem.Web.Controllers.Generic;
using Microsoft.AspNet.Identity;

namespace LearningSystem.Web.Areas.Admin.Controllers
{
    [Authorize(Roles= "Administrator")]
    public class CoursesManageController 
        : CrudController
        <CoursesService<UnitOfWork>, CourseModifyViewModel, CourseFilterViewModel, CourseIndexViewModel, CourseDetailsViewModel, Course>
    {
        [HttpGet]
        public ActionResult Details(int id)
        {
            return base.details(c => c.Id == id);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return base.edit(c => c.Id == id);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return base.delete(c => c.Id == id);
        }

        [HttpPost]
        public ActionResult ConfirmDelete(int id, string returnUrl)
        {
            return base.confirmDelete(c => c.Id == id, returnUrl);
        }
    }
}
