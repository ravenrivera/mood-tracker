using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.OpenApi.Extensions;
using MoodTrackerBackend.Models;

namespace MoodTrackerBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyMoodPostModelsController : ControllerBase
    {
        private readonly DailyMoodPostContext _context;

        public DailyMoodPostModelsController(DailyMoodPostContext context)
        {
            _context = context;
        }

        // GET: api/DailyMoodPostModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyMoodEntriesDto>>> GetDailyMoodEntries()
        {
          if (_context.DailyMoodEntries == null)
          {
              return NotFound();
          }

          var dailyMoodPosts = await _context.DailyMoodEntries.ToListAsync();
          var returnableEntries = new List<DailyMoodEntriesDto>();
          foreach (var dailyMoodPost in dailyMoodPosts)
          {
              List<ActivitiesDto> activitiesList =
                  await _context.ActivityEntries.Where(a => a.PostId == dailyMoodPost.Id).Select(a => new ActivitiesDto(){ Id = a.Id, DailyActivity = a.DailyActivity.ToString()}).ToListAsync();
              
              returnableEntries.Add(new DailyMoodEntriesDto()
              {
                  Id = dailyMoodPost.Id,
                  CreateOn = dailyMoodPost.CreateOn,
                  Thoughts = dailyMoodPost.Thoughts,
                  CurrentMood = dailyMoodPost.CurrentMood.ToString(),
                  DailyActivity = activitiesList
              });
          }

          return returnableEntries;
        }

        // GET: api/DailyMoodPostModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DailyMoodPostModel>> GetDailyMoodPostModel(Guid id)
        {
          if (_context.DailyMoodEntries == null)
          {
              return NotFound();
          }
          var dailyMoodPostModel = await _context.DailyMoodEntries.FindAsync(id);

          if (dailyMoodPostModel == null)
          {
              return NotFound();
          }

          return dailyMoodPostModel;
        }

        // PUT: api/DailyMoodPostModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDailyMoodPostModel(Guid id, DailyMoodEntriesDto dailyMoodEntry)
        {
            if (id != dailyMoodEntry.Id)
            {
                return BadRequest();
            }

            var moodPostToUpdate = _context.DailyMoodEntries.Find(id);

            if (moodPostToUpdate.CurrentMood != Enum.Parse<Mood>(dailyMoodEntry.CurrentMood))
            {
                moodPostToUpdate.CurrentMood = Enum.Parse<Mood>(dailyMoodEntry.CurrentMood);
            }

            if (moodPostToUpdate.Thoughts != dailyMoodEntry.Thoughts)
            {
                moodPostToUpdate.Thoughts = dailyMoodEntry.Thoughts;
            }

            _context.Entry(moodPostToUpdate).State = EntityState.Modified;

            foreach (var activity in dailyMoodEntry.DailyActivity)
            {
                var activityEntryToUpdate = _context.ActivityEntries.Find(activity.Id);

                if (activityEntryToUpdate.DailyActivity != Enum.Parse<Activities>(activity.DailyActivity))
                {
                    activityEntryToUpdate.DailyActivity = Enum.Parse<Activities>(activity.DailyActivity);
                    _context.Entry(activityEntryToUpdate).State = EntityState.Modified;
                }
                else
                {
                    _context.ActivityEntries.Add(new ActivitiesModel()
                    {
                        Id = Guid.NewGuid(),
                        PostId = dailyMoodEntry.Id,
                        DailyActivity = Enum.Parse<Activities>(activity.DailyActivity)
                    });
                }

            }



            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DailyMoodPostModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DailyMoodPostModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DailyMoodEntriesDto>> PostDailyMoodPost(DailyMoodEntriesDto dailyMoodEntry)
        {

            if (_context.DailyMoodEntries == null)
            {
                return NotFound();
            }
          
            DailyMoodPostModel newDailyPostEntry = new DailyMoodPostModel()
            {
                CreateOn = dailyMoodEntry.CreateOn,
                Thoughts = dailyMoodEntry.Thoughts,
                CurrentMood = Enum.Parse<Mood>(dailyMoodEntry.CurrentMood.ToLower())
            };
              
            List<ActivitiesModel> activitiesList = new List<ActivitiesModel>();
            foreach (var activity in dailyMoodEntry.DailyActivity)
            {
                ActivitiesModel entry = new ActivitiesModel()
                {
                    PostId = newDailyPostEntry.Id,
                    DailyActivity = Enum.Parse<Activities>(activity.DailyActivity.ToLower())
                };

                activitiesList.Add(entry);
            }

            _context.DailyMoodEntries.Add(newDailyPostEntry);
            _context.ActivityEntries.AddRange(activitiesList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDailyMoodPostModel", new { id = newDailyPostEntry.Id }, dailyMoodEntry);
        }

        // DELETE: api/DailyMoodPostModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDailyMoodPostModel(Guid id)
        {
            if (_context.DailyMoodEntries == null)
            {
                return NotFound();
            }
            var dailyMoodPostModel = await _context.DailyMoodEntries.FindAsync(id);
            if (dailyMoodPostModel == null)
            {
                return NotFound();
            }
            List<ActivitiesModel> removedList = _context.ActivityEntries.Where(a => a.PostId == id).ToList();
            
            _context.DailyMoodEntries.Remove(dailyMoodPostModel);
            _context.ActivityEntries.RemoveRange(removedList);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("postsCreatedOn/{createdOnDate}")]
        public async Task<ActionResult<IEnumerable<DailyMoodEntriesDto>>> GetDailyPostsCreatedOn(string createdOnDate)
        {
            var allPosts = _context.DailyMoodEntries.ToList();
            DateTime parsedCreatedOnDate = DateTime.Parse(createdOnDate);

            List<DailyMoodEntriesDto> returnableEntriesDtos = new List<DailyMoodEntriesDto>();

            foreach (var entry in allPosts)
            {
                if(entry.CreateOn.Date == parsedCreatedOnDate)
                {
                    List<ActivitiesModel> matchingActivities = _context.ActivityEntries.Where(a => a.PostId == entry.Id).ToList();
                    List<ActivitiesDto> transformedActivities = matchingActivities.Select(activity => new ActivitiesDto() {Id = activity.Id, DailyActivity = activity.DailyActivity.ToString() }).ToList();

                    returnableEntriesDtos.Add(new DailyMoodEntriesDto()
                    {
                        Id = entry.Id,
                        CreateOn = entry.CreateOn,
                        CurrentMood = entry.CurrentMood.ToString(),
                        Thoughts = entry.Thoughts,
                        DailyActivity = transformedActivities
                    });
                }
            }
            return returnableEntriesDtos;

        }

        private bool DailyMoodPostModelExists(Guid id)
        {
            return (_context.DailyMoodEntries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
