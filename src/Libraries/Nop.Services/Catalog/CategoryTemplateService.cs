﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Services.Caching;
using Nop.Services.Caching.Extensions;
using Nop.Services.Events;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Category template service
    /// </summary>
    public partial class CategoryTemplateService : ICategoryTemplateService
    {
        #region Fields

        private readonly ICacheKeyService _cacheKeyService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<CategoryTemplate> _categoryTemplateRepository;

        #endregion

        #region Ctor

        public CategoryTemplateService(ICacheKeyService cacheKeyService,
        IEventPublisher eventPublisher,
            IRepository<CategoryTemplate> categoryTemplateRepository)
        {
            _cacheKeyService = cacheKeyService;
            _eventPublisher = eventPublisher;
            _categoryTemplateRepository = categoryTemplateRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete category template
        /// </summary>
        /// <param name="categoryTemplate">Category template</param>
        public virtual async Task DeleteCategoryTemplate(CategoryTemplate categoryTemplate)
        {
            if (categoryTemplate == null)
                throw new ArgumentNullException(nameof(categoryTemplate));

            await _categoryTemplateRepository.Delete(categoryTemplate);

            //event notification
            await _eventPublisher.EntityDeleted(categoryTemplate);
        }

        /// <summary>
        /// Gets all category templates
        /// </summary>
        /// <returns>Category templates</returns>
        public virtual async Task<IList<CategoryTemplate>> GetAllCategoryTemplates()
        {
            var query = from pt in _categoryTemplateRepository.Table
                        orderby pt.DisplayOrder, pt.Id
                        select pt;

            var templates = await query.ToCachedList(_cacheKeyService.PrepareKeyForDefaultCache(NopCatalogDefaults.CategoryTemplatesAllCacheKey));

            return templates;
        }

        /// <summary>
        /// Gets a category template
        /// </summary>
        /// <param name="categoryTemplateId">Category template identifier</param>
        /// <returns>Category template</returns>
        public virtual async Task<CategoryTemplate> GetCategoryTemplateById(int categoryTemplateId)
        {
            if (categoryTemplateId == 0)
                return null;

            return await _categoryTemplateRepository.ToCachedGetById(categoryTemplateId);
        }

        /// <summary>
        /// Inserts category template
        /// </summary>
        /// <param name="categoryTemplate">Category template</param>
        public virtual async Task InsertCategoryTemplate(CategoryTemplate categoryTemplate)
        {
            if (categoryTemplate == null)
                throw new ArgumentNullException(nameof(categoryTemplate));

            await _categoryTemplateRepository.Insert(categoryTemplate);

            //event notification
            await _eventPublisher.EntityInserted(categoryTemplate);
        }

        /// <summary>
        /// Updates the category template
        /// </summary>
        /// <param name="categoryTemplate">Category template</param>
        public virtual async Task UpdateCategoryTemplate(CategoryTemplate categoryTemplate)
        {
            if (categoryTemplate == null)
                throw new ArgumentNullException(nameof(categoryTemplate));

            await _categoryTemplateRepository.Update(categoryTemplate);

            //event notification
            await _eventPublisher.EntityUpdated(categoryTemplate);
        }

        #endregion
    }
}