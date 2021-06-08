using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SBDA.API.Repository;
using SBDA.API.IServices;
using SBDA.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SBDA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodGroupController : ControllerBase
    {
        private readonly IBloodGroupService _bloodGroupService;

        public BloodGroupController(IBloodGroupService bloodGroupService)
        {
            _bloodGroupService = bloodGroupService;
        }

        //api/bloodgroup/allbloodgroups
        [HttpGet("allbloodgroups")]
        public async Task<ActionResult<BloodGroup>> GetAllBloodGroups()
        {
            try
            {
                return Ok(await _bloodGroupService.GetAllBloodGroupsAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
           // return await _bloodGroupService.GetAllBloodGroupsAsync();
        }

        //[HttpGet("getbloodgroup")]
        [HttpGet("{bloodGroupId:int}")]
        public async Task<ActionResult<BloodGroup>> GetBloodGroupById(int bloodGroupId)
        {
            try
            {
                var result = await _bloodGroupService.GetBloodGroupByIdAsync(bloodGroupId);

                if (result == null)
                {
                    return NotFound();
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost("addbloodGroup")]
        public async Task<ActionResult> AddBloodGroup(BloodGroup model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest();
                }
                var result = await _bloodGroupService.AddBloodGroupAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);//Status code 200
                }
                else
                {
                    return BadRequest(result);
                }

                //var result = await _member.GetMemberByMobile(member.Mobile);
                //if (result != null)
                //{
                //    ModelState.AddModelError("Mobile", "Phone number already exist");
                //    return BadRequest(ModelState);
                //}
               // BaseAPIResponse 
                
                //return CreatedAtAction(nameof(GetBloodGroupById), new { id = addedBloodGroup.BloodGroupID }, addedBloodGroup);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());

            }

        }
        [HttpPut("updateBloodGroup")]
        public async Task<ActionResult> UpdateBloodGroup(BloodGroup model)
        {
            try
            {
                var result = await _bloodGroupService.GetBloodGroupByIdAsync(model.BloodGroupID);
                if (result == null)
                {
                    return NotFound($"Blood Group={model.BloodGroupName} not found");
                }
                var response= await _bloodGroupService.UpdateBloodGroupAsync(model);
                if (response.IsSuccess)
                {
                    return Ok(response);//Status code 200
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());

            }
        }
        [HttpDelete("{bloodGroupId:int}")]
        public async Task<ActionResult> DeleteBloodGroup(int bloodGroupId)
        {
            try
            {
                var result = await _bloodGroupService.GetBloodGroupByIdAsync(bloodGroupId);
                if (result == null)
                {
                    return NotFound($"Blood group with id={bloodGroupId} not found");
                }
                var response= await _bloodGroupService.DeleteBloodGroupByIdAsync(bloodGroupId);
                if (response.IsSuccess)
                {
                    return Ok(response);//Status code 200
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());

            }
        }
    }
}
