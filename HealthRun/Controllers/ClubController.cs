using Microsoft.AspNetCore.Mvc;
using HealthRun.Data.Enum;
using HealthRun.Helpers;
using HealthRun.Interfaces;
using HealthRun.Models;
using HealthRun.Repository;
using HealthRun.ViewModels;
using System.Xml.Linq;

namespace HealthRun.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("RunningClubs")]
        public async Task<IActionResult> Index(int category = -1, int page = 1, int pageSize = 6)
        {
            if (page < 1 || pageSize < 1)
            {
                return NotFound();
            }

            // if category is -1 (All) dont filter else filter by selected category
            var clubs = category switch
            {
                -1 => await _clubRepository.GetSliceAsync((page - 1) * pageSize, pageSize),
                _ => await _clubRepository.GetClubsByCategoryAndSliceAsync((ClubCategory)category, (page - 1) * pageSize, pageSize),
            };

            var count = category switch
            {
                -1 => await _clubRepository.GetCountAsync(),
                _ => await _clubRepository.GetCountByCategoryAsync((ClubCategory)category),
            };

            var clubViewModel = new IndexClubViewModel
            {
                Clubs = clubs,
                Page = page,
                PageSize = pageSize,
                TotalClubs = count,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                Category = category,
            };

            return View(clubViewModel);
        }

        //[HttpGet]
        //[Route("RunningClubs/{state}")]
        //public async Task<IActionResult> ListClubsByState(string state)
        //{
        //    var clubs = await _clubRepository.GetClubsByState(StateConverter.GetStateByName(state).ToString());
        //    var clubVM = new ListClubByStateViewModel()
        //    {
        //        Clubs = clubs
        //    };
        //    if (clubs.Count() == 0)
        //    {
        //        clubVM.NoClubWarning = true;
        //    }
        //    else
        //    {
        //        clubVM.State = state;
        //    }
        //    return View(clubVM);
        //}

        //[HttpGet]
        //[Route("RunningClubs/{city}/{state}")]
        //public async Task<IActionResult> ListClubsByCity(string city, string state)
        //{
        //    var clubs = await _clubRepository.GetClubByCity(city);
        //    var clubVM = new ListClubByCityViewModel()
        //    {
        //        Clubs = clubs
        //    };
        //    if (clubs.Count() == 0)
        //    {
        //        clubVM.NoClubWarning = true;
        //    }
        //    else
        //    {
        //        clubVM.State = state;
        //        clubVM.City = city;
        //    }
        //    return View(clubVM);
        //}

        [HttpGet]
        [Route("event/{runningClub}/{id}")]
        public async Task<IActionResult> DetailClub(int id, string runningClub)
        {
            var club = await _clubRepository.GetByIdAsync(id);

            return club == null ? NotFound() : View(club);
        }

        //[HttpGet]
        //[Route("RunningClubs/State")]
        //public async Task<IActionResult> RunningClubsByStateDirectory()
        //{
        //    var states = await _clubRepository.GetAllStates();
        //    var clubVM = new RunningClubByState()
        //    {
        //        States = states
        //    };

        //    return states == null ? NotFound() : View(clubVM);
        //}

        //[HttpGet]
        //[Route("RunningClubs/State/City")]
        //public async Task<IActionResult> RunningClubsByStateForCityDirectory()
        //{
        //    var states = await _clubRepository.GetAllStates();
        //    var clubVM = new RunningClubByState()
        //    {
        //        States = states
        //    };

        //    return states == null ? NotFound() : View(clubVM);
        //}

        //[HttpGet]
        //[Route("RunningClubs/{state}/City")]
        //public async Task<IActionResult> RunningClubsByCityDirectory(string state)
        //{
        //    var cities = await _clubRepository.GetAllCitiesByState(StateConverter.GetStateByName(state).ToString());
        //    var clubVM = new RunningClubByCity()
        //    {
        //        Cities = cities
        //    };

        //    return cities == null ? NotFound() : View(clubVM);
        //}

        [HttpGet]
        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel { AppUserId = curUserId };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                //var result = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = "Photo",//result.Url.ToString(),
                    ClubCategory = clubVM.ClubCategory,
                    AppUserId = clubVM.AppUserId,
                    StartDate=clubVM.StartDate,
                    EndDate=clubVM.EndDate,
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,
                    }
                };
                //check if the start date is less than the end date
                if (clubVM.StartDate <= club.EndDate)
                {

                    _clubRepository.Add(club);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("","Start Date Should Less Than End Date");
                }
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(clubVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if (club == null) return View("Error");
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                StartDate = club.StartDate,
                EndDate = club.EndDate,
                URL = club.Image,
                ClubCategory = club.ClubCategory
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {

            var userId = User.GetUserId();

            if (ModelState.IsValid)
            {


                var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);

                if (userClub == null)
                {
                    return View("Error");
                }

                //var photoResult = await _photoService.AddPhotoAsync(raceVM.Image);

                //if (photoResult.Error != null)
                //{
                //    ModelState.AddModelError("Image", "Photo upload failed");
                //    return View(raceVM);
                //}

                //if (!string.IsNullOrEmpty(userRace.Image))
                //{
                //    _ = _photoService.DeletePhotoAsync(userRace.Image);
                //}

                var club = new Club
                {

                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = "photo",//photoResult.Url.ToString(),
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address,
                    StartDate = clubVM.StartDate,
                    EndDate = clubVM.EndDate,
                    ClubCategory = clubVM.ClubCategory,
                    AppUserId = userId
                };    //check if the start date is less than the end date
                if (clubVM.StartDate <= clubVM.EndDate)
                {
                    _clubRepository.Update(club);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Start Date Should Less Than End Date");
                }
            }
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails == null)
            {
                return View("Error");
            }
            if (!string.IsNullOrEmpty(clubDetails.Image))
            {
                _ = _photoService.DeletePhotoAsync(clubDetails.Image);
            }
            _clubRepository.Delete(clubDetails);
            return RedirectToAction("Index");
        }

        //[HttpPost, ActionName("Delete")]
        //public async Task<IActionResult> DeleteClub(int id)
        //{
        //    var clubDetails = await _clubRepository.GetByIdAsync(id);

        //    if (clubDetails == null)
        //    {
        //        return View("Error");
        //    }

        //    if (!string.IsNullOrEmpty(clubDetails.Image))
        //    {
        //        _ = _photoService.DeletePhotoAsync(clubDetails.Image);
        //    }

        //    _clubRepository.Delete(clubDetails);
        //    return RedirectToAction("Index");
        //}
    }
}