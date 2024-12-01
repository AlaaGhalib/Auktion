using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Auktion.Core.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Auktion.Persistence
{
    public class GenericPersistence<TDomain, TDb> : IGenericPersistence<TDb,TDomain> 
        where TDomain : class 
        where TDb : class
    {
        private readonly AuctionDbContext _context;
        private readonly DbSet<TDb> _dbSet;
        private readonly IMapper _mapper;

        public GenericPersistence(AuctionDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = _context.Set<TDb>();
            _mapper = mapper;
        }

        public Collection<TDomain> Get(Expression<Func<TDb, bool>> filter = null, params Expression<Func<TDb, object>>[] includes)
        {
            IQueryable<TDb> query = _dbSet.AsNoTracking();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var dbEntities = query.ToList();
            var domainEntities = _mapper.Map<List<TDomain>>(dbEntities);
            return new Collection<TDomain>(domainEntities);
        }

        public TDomain GetById(int id, params Expression<Func<TDb, object>>[] includes)
        {
            IQueryable<TDb> query = _dbSet.AsNoTracking();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var primaryKeyProperty = typeof(TDb).GetProperties()
                .FirstOrDefault(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Any());
            if (primaryKeyProperty == null)
            {
                throw new InvalidOperationException("Primary key not found.");
            }
            var dbEntity = query.FirstOrDefault(e => EF.Property<int>(e, primaryKeyProperty.Name) == id);
            return (dbEntity == null ? null : _mapper.Map<TDomain>(dbEntity)) ?? throw new InvalidOperationException();
        }


        public void Create(TDomain domainEntity)
        {
            ArgumentNullException.ThrowIfNull(domainEntity);
            var dbEntity = _mapper.Map<TDb>(domainEntity); 
            _dbSet.Add(dbEntity);
            _context.SaveChanges();
        }

        public void Update(TDomain domainEntity)
        {
            ArgumentNullException.ThrowIfNull(domainEntity);
            var dbEntity = _mapper.Map<TDb>(domainEntity); 
            _dbSet.Attach(dbEntity);
            _context.Entry(dbEntity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Remove(int id)
        {
            var dbEntity = _dbSet.Find(id); 
            if (dbEntity == null) return;
            _dbSet.Remove(dbEntity);
            _context.SaveChanges();
        }
    }
}
