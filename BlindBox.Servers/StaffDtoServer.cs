using AutoMapper;
using AutoMapper.QueryableExtensions;
using BilndBox.Dto.Entity;
using BlindBox.EF;
using BlindBox.IServers.IDtoServers;
using BlindBox.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BlindBox.Servers
{
    public class StaffDtoServer : IStaffDtoService
    {
        private MyDbContext _context;
        private IMapper _mapper;
        private MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.CreateProjection<Staff, StaffDto>());
        public StaffDtoServer(MyDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        /// <summary>
        /// 获取全部的StaffDto数据
        /// </summary>
        public IQueryable<StaffDto> ModelQueryable => GetAllStaffDto();

        /// <summary>
        /// 将全部的Staff数据转换为StaffDto数据
        /// </summary>
        /// <returns></returns>
        public IQueryable<StaffDto> GetAllStaffDto()
        {
            try
            {
                var result = _context.Staffs.AsNoTracking().ProjectTo<StaffDto>(configuration);
                if (result == null)
                {
                    Log.Warning("获取全部的Staff数据：数据为Null");
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

        /// <summary>
        /// 新增一个Staff数据
        /// </summary>
        /// <param name="t">StaffDto类型的数据</param>
        /// <returns>添加后的StaffDto，失败为Null</returns>
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

        /// <summary>
        /// 软删除一个Staff数据
        /// </summary>
        /// <param name="t">ID</param>
        /// <returns>是否成功</returns>
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

        /// <summary>
        /// 根据编号获取一条StaffDto数据
        /// </summary>
        /// <param name="t">ID</param>
        /// <returns>一条StaffDto数据,可能为空</returns>
        public async Task<StaffDto> SingAsync(int? t)
        {
            return await ModelQueryable.AsNoTracking().SingleOrDefaultAsync(m => m.StaffId == t);
        }

        /// <summary>
        /// 更改一条Staff数据
        /// </summary>
        /// <param name="t">StaffDto类型的数据</param>
        /// <returns>更改后的StaffDto，失败为Null</returns>
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

        /// <summary>
        /// 根据名称进行模糊查询
        /// </summary>
        /// <param name="name">搜索值</param>
        /// <returns>与搜索值有关的Staff列表</returns>
        public async Task<List<StaffDto>> FuzzyAsync(string name)
        {
            var users = await ModelQueryable.AsNoTracking().
                Where(u => Microsoft.EntityFrameworkCore.EF.Functions.Like(u.StaffName, $"%{name}%"))
                .ToListAsync();
            return users;
        }
    }
}