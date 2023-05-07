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
    public class GradeDtoServer : IGradeDtoService
    {
        private MyDbContext _context;
        private IMapper _mapper;
        private MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.CreateProjection<Grade, GradeDto>());
        public GradeDtoServer(MyDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public IQueryable<GradeDto> ModelQueryable => GetAllGradeDto();

        public IQueryable<GradeDto> GetAllGradeDto()
        {
            try
            {
                var result = _context.Grades.AsNoTracking().ProjectTo<GradeDto>(configuration);
                if (result == null)
                {
                    Log.Warning("获取全部的GradeDto数据：数据为Null");
                }
                else
                {
                    Log.Information("获取全部的GradeDto数据：成功");
                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "获取全部的GradeDto数据：异常");
                return null;
            }
        }

        public async Task<GradeDto> CreateAsync(GradeDto t)
        {
            try
            {
                var grade = _mapper.Map<Grade>(t);
                _context.Add(grade);
                int r = await _context.SaveChangesAsync();
                if (r == 0)
                {
                    Log.Warning($"添加一条Grade数据：未添加数据 id:{t.GradeId}");
                }
                else
                {
                    Log.Information($"添加一条Grade数据：成功 id:{t.GradeId}");
                }
                return r > 0 ? t : null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"添加一条Staff数据：异常 id:{t.GradeId}");
                return null;
            }
            
        }

        public async Task<bool> DeleteAsync(int? t)
        {
            var gradeDto = await SingAsync(t);

            if (gradeDto != null)
            {
                var grade = _mapper.Map<Grade>(gradeDto);
                grade.IsDelete = true;
                _context.Update(grade);
                int r = await _context.SaveChangesAsync();
                return r > 0;
            }
            return false;
        }

        public async Task<GradeDto> SingAsync(int? t)
        {
            return await ModelQueryable.AsNoTracking().SingleOrDefaultAsync(m => m.GradeId == t);
        }

        public async Task<GradeDto> UpdateAsync(GradeDto t)
        {

            var gradeDto = await SingAsync(t.GradeId);

            if (gradeDto != null)
            {
                var grade = _mapper.Map<Grade>(t);
                _context.Update(grade);
                int r = await _context.SaveChangesAsync();
                return r > 0 ? t : null;
            }
            throw new NotImplementedException();
        }

        public Task<List<GradeDto>> FuzzyAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}