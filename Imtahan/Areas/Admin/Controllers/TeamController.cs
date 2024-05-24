using Bussiness.Abstracts;
using Bussiness.Exceptions;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Imtahan.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = "Admin")]

	public class TeamController : Controller
    {
        ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public IActionResult Index()
        {
            var teams= _teamService.GetAllTeams();
            return View(teams);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Team team)
        {
            try
            {
                _teamService.Create(team);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch(NotNullException ex)
            {
                ModelState.AddModelError(ex.Property,ex.Message);
                return View();
            }
            catch (FileContentTypeException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View();
            }
            catch (FileSizeException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            try
            {
                _teamService.Delete(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            var updateTeam = _teamService.GetTeam();
            if(updateTeam == null)
            {
                return NotFound();
            }
            return View(updateTeam);
        }
        [HttpPost]
        public IActionResult Update(Team team)
        {
            try
            {
                _teamService.Update(team.Id,team);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (FileContentTypeException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View();
            }
            catch (FileSizeException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View();
            }
            return RedirectToAction("Index");

        }
    }
}
