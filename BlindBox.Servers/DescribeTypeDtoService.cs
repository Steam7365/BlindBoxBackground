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
    public class DescribeTypeDtoService : IDescribeTypeDtoService
    {
        private MyDbContext _context;
        private IMapper _mapper;
        private MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.CreateProjection<DescribeType, DescribeTypeDto>());
        public DescribeTypeDtoService(MyDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        /// <summary>
        /// 获取全部的DescribeTypeDto数据
        /// </summary>
        public IQueryable<DescribeTypeDto> ModelQueryable => GetAllDescribeTypeDto();

        /// <summary>
        /// 将全部的DescribeType数据转换为DescribeTypeDto数据
        /// </summary>
        /// <returns></returns>
        public IQueryable<DescribeTypeDto> GetAllDescribeTypeDto()
        {
            try
            {
                var result = _context.DescribeTypes.AsNoTracking().ProjectTo<DescribeTypeDto>(configuration);
                if (result == null)
                {
                    Log.Warning("获取全部的DescribeTypeDto数据：数据为Null");
                }
                else
                {
                    Log.Information("获取全部的DescribeTypeDto数据：成功");
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "获取全部的DescribeTypeDto数据：异常");
                return null;
            }
        }

        /// <summary>
        /// 新增一个DescribeType数据
        /// </summary>
        /// <param name="t">DescribeTypeDto类型的数据</param>
        /// <returns>添加后的DescribeTypeDto，失败为Null</returns>
        public async Task<DescribeTypeDto> CreateAsync(DescribeTypeDto t)
        {
            try
            {
                var describeType = _mapper.Map<DescribeType>(t);
                _context.Add(describeType);
                int r = await _context.SaveChangesAsync();
                if (r == 0)
                {
                    Log.Warning($"添加一条DescribeType数据：未添加数据 id:{t.DescribeTypeId}");
                }
                else
                {
                    Log.Information($"添加一条DescribeType数据：成功 id:{t.DescribeTypeId}");
                }
                return r > 0 ? t : null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"添加一条DescribeType数据：异常 id:{t.DescribeTypeId}");
                return null;
            }
            
        }

        /// <summary>
        /// 软删除一个DescribeType数据
        /// </summary>
        /// <param name="t">ID</param>
        /// <returns>是否成功</returns>
        public async Task<bool> DeleteAsync(int? t)
        {
            var describeTypeDto = await SingAsync(t);

            if (describeTypeDto != null)
            {
                var describeType = _mapper.Map<DescribeType>(describeTypeDto);
                describeType.IsDelete = true;
                _context.Update(describeType);
                int r = await _context.SaveChangesAsync();
                return r > 0;
            }
            return false;
        }

        /// <summary>
        /// 根据编号获取一条DescribeTypeDto数据
        /// </summary>
        /// <param name="t">ID</param>
        /// <returns>一条DescribeTypeDto数据,可能为空</returns>
        public async Task<DescribeTypeDto> SingAsync(int? t)
        {
            return await ModelQueryable.AsNoTracking().SingleOrDefaultAsync(m => m.DescribeTypeId == t);
        }

        /// <summary>
        /// 更改一条DescribeType数据
        /// </summary>
        /// <param name="t">DescribeTypeDto类型的数据</param>
        /// <returns>更改后的DescribeTypeDto，失败为Null</returns>
        public async Task<DescribeTypeDto> UpdateAsync(DescribeTypeDto t)
        {

            var describeTypeDto = await SingAsync(t.DescribeTypeId);

            if (describeTypeDto != null)
            {
                var DescribeType = _mapper.Map<DescribeType>(t);
                _context.Update(DescribeType);
                int r = await _context.SaveChangesAsync();
                return r > 0 ? t : null;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据名称进行模糊查询
        /// </summary>
        /// <param name="name">搜索值</param>
        /// <returns>与搜索值有关的DescribeType列表</returns>
        public async Task<List<DescribeTypeDto>> FuzzyAsync(string title)
        {
            var users = await ModelQueryable.AsNoTracking().
                Where(u => Microsoft.EntityFrameworkCore.EF.Functions.Like(u.DescTitle, $"%{title}%"))
                .ToListAsync();
            return users;
        }
    }
}