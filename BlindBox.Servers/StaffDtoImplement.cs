using AutoMapper;
using AutoMapper.QueryableExtensions;
using BilndBox.Dto;
using BlindBox.EF;
using BlindBox.IServers.IDtoServers;
using BlindBox.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BlindBox.Servers
{
    public class StaffDtoImplement : IStaffDtoService
    {
        private MyDbContext _context;
        private IMapper _mapper;
        private MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.CreateProjection<Staff, StaffDto>());
        public StaffDtoImplement(MyDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public IQueryable<StaffDto> ModelQueryable => GetAllStaffDto();

        public IQueryable<StaffDto> GetAllStaffDto()
        {
            try
            {
                var result = _context.Staffs.ProjectTo<StaffDto>(configuration);
                if (result == null)
                {
                    Log.Warning("获取全部的Staff数据：未获取数据");
                }
                else
                {
                    Log.Information("获取全部的Staff数据：成功");
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "获取全部的Staff数据：异常");
                return null;
            }
        }

        public async Task<StaffDto> CreateAsync(StaffDto t)
        {
            try
            {
                var staff = _mapper.Map<Staff>(t);
                _context.Add(staff);
                int r = await _context.SaveChangesAsync();
                if (r == 0)
                {
                    Log.Warning($"添加一条Staff数据：未添加数据 id:{t.StaffId}");
                }
                else
                {
                    Log.Information($"添加一条Staff数据：成功 id:{t.StaffId}");
                }
                return r > 0 ? t : null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"添加一条Staff数据：异常 id:{t.StaffId}");
                return null;
            }
            
        }

        public async Task<bool> DeleteAsync(int? t)
        {
            var staffDto = await SingAsync(t);

            if (staffDto != null)
            {
                var staff = _mapper.Map<Staff>(staffDto);
                staff.StaffState = "离职";
                staff.IsDelete = true;
                _context.Update(staff);
                int r = await _context.SaveChangesAsync();
                return r > 0;
            }
            return false;
        }

        public async Task<StaffDto> SingAsync(int? t)
        {
            return await ModelQueryable.AsNoTracking().SingleOrDefaultAsync(m => m.StaffId == t);
        }

        public async Task<StaffDto> UpdateAsync(StaffDto t)
        {

            var staffDto = await SingAsync(t.StaffId);

            if (staffDto != null)
            {
                var staff = _mapper.Map<Staff>(t);
                _context.Update(staff);
                int r = await _context.SaveChangesAsync();
                return r > 0 ? t : null;
            }
            throw new NotImplementedException();
        }

        public Task<List<StaffDto>> FuzzyAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}