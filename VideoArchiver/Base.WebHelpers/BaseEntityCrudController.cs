using Contracts.DAL;
using Domain.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Base.WebHelpers;

public abstract class
    BaseEntityCrudController<TAppUnitOfWork, TDomainEntity, TEntity> : BaseEntityCrudController<TAppUnitOfWork,
        TDomainEntity, TEntity, Guid>
    where TAppUnitOfWork : IBaseUnitOfWork
    where TEntity : class, IIdDatabaseEntity<Guid>
    where TDomainEntity : class, IIdDatabaseEntity<Guid>
{
    protected BaseEntityCrudController(TAppUnitOfWork uow) : base(uow)
    {
    }

    protected override Guid NewKey()
    {
        return Guid.NewGuid();
    }
}

public abstract class BaseEntityCrudController<TAppUnitOfWork, TDomainEntity, TEntity, TKey> : Controller
    where TAppUnitOfWork : IBaseUnitOfWork
    where TEntity : class, IIdDatabaseEntity<TKey>
    where TKey : struct, IEquatable<TKey>
    where TDomainEntity : class, IIdDatabaseEntity<TKey>
{
    protected readonly TAppUnitOfWork Uow;

    public BaseEntityCrudController(TAppUnitOfWork uow)
    {
        Uow = uow;
    }

    protected abstract IBaseEntityRepository<TDomainEntity, TEntity, TKey> Entities { get; }

    protected abstract TKey NewKey();

    // GET: Entity
    public async Task<IActionResult> Index()
    {
        return View(await Entities.GetAllAsync());
    }

    // GET: Entity/Details/5
    public async Task<IActionResult> Details(TKey? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var entity = await Entities.GetByIdAsync(id.Value);
        if (entity == null)
        {
            return NotFound();
        }

        return View(entity);
    }

    protected virtual Task SetupViewData(TEntity? entity = null)
    {
        return Task.CompletedTask;
    }

    // GET: Entity/Create
    public async Task<IActionResult> Create()
    {
        await SetupViewData();
        return View();
    }

    // POST: Entity/Create
    // Requires creation of separate public Create method in derived class
    // That method should have [HttpPost] [ValidateAntiForgeryToken] attributes
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    protected async Task<IActionResult> CreateInternal(TEntity entity)
    {
        if (ModelState.IsValid)
        {
            entity.Id = NewKey();
            Entities.Add(entity);
            await Uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        await SetupViewData(entity);
        return View(entity);
    }

    // GET: Entity/Edit/5
    public async Task<IActionResult> Edit(TKey? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var entity = await Entities.GetByIdAsync(id.Value);
        if (entity == null)
        {
            return NotFound();
        }

        await SetupViewData(entity);
        return View(entity);
    }

    // POST: Entity/Edit/5
    // Requires creation of separate public Edit method in derived class
    // That method should have [HttpPost] [ValidateAntiForgeryToken] attributes
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    public async Task<IActionResult> EditInternal(TKey id, TEntity entity)
    {
        if (!id.Equals(entity.Id))
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                Entities.Update(entity);
                await Uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await Entities.ExistsAsync(entity.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        await SetupViewData(entity);
        return View(entity);
    }

    // GET: Entity/Delete/5
    public async Task<IActionResult> Delete(TKey? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var entity = await Entities.GetByIdAsync(id.Value);
        if (entity == null)
        {
            return NotFound();
        }

        return View(entity);
    }

    // POST: Entity/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(TKey id)
    {
        await Entities.RemoveAsync(id);
        await Uow.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}