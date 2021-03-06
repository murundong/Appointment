﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appoint.EntityFramework;
using Appoint.EntityFramework.Data;
using Appoint.EntityFramework.DbContextProvider;
using Appoint.EntityFramework.Rep;
using Appoint.EntityFramework.Uow;
using Appoint.EntityFramework.ViewData;

namespace Appoint.Application.Services
{
    public class CardTemplateService : ICardTemplateService
    {
       
        public IRepository<App_DbContext, CardTemplate> _repository { get; set; }
        public IRepository<App_DbContext, Doors> _repositoryDoors { get; set; }
        public IRepository<App_DbContext, View_PrevCardTemplateOutput> _repositoryPrevTemplate { get; set; }
        public IUnitOfWork<App_DbContext> uof { get; set; }

        public CardTemplate CreateCardTemplate(CardTemplate model)
        {
            _repository.Insert(model);
            if (uof.SaveChange() > 0) return model;
            return null;
        }

      
        public List<View_PrevCardTemplateOutput> GetAllDoorCardsTemplate(int doorId)
        {
            string sql = @"select [door_img]=(select door_img from [dbo].[Doors] where id =[dbo].[CardTemplate].door_id ), * from [dbo].[CardTemplate]
                            where door_id = @doorId";
            var SqlParam = new SqlParameter[] {
                new SqlParameter("@doorId",doorId)
            };
            var res = _repositoryPrevTemplate.ExecuteSqlQuery(sql,SqlParam);
            return res.ToList();
        }

        public CardTemplate GetCardsTemplateById(int id)
        {
            return _repository.FirstOrDefault(s => s.id == id);
        }

        public List<ViewDoorCardsSelect> GetDoorCards(int doorId)
        {
            var res= _repository.GetAll().Where(s => s.door_id == doorId);
            return AutoMapper.Mapper.Map<List<ViewDoorCardsSelect>>(res);
        }

        public Base_PageOutput<View_CardTemplateOutputItem> PageCardTemplate(View_CardTemplateInput input)
        {
            Base_PageOutput<View_CardTemplateOutputItem> res = new Base_PageOutput<View_CardTemplateOutputItem>() {  data = new View_CardTemplateOutputItem() {  temps = new List<View_CardTemplateOutput>()} };
            if (input == null || input.door_id <= 0) return res;
            var query = _repository.GetAll()
                .Where(s => s.door_id == input.door_id);
            res.total = query.Count();
            var query_end = query.OrderByDescending(s=>s.create_time)
                .Skip((input.page_index - 1) * input.page_size)
                .Take(input.page_size);
            res.data.temps = AutoMapper.Mapper.Map<List<View_CardTemplateOutput>>(query_end);
            res.data.img = _repositoryDoors.FirstOrDefault(s=>s.id == input.door_id)?.door_img;
            return res;
        }

        public bool UpdateCardTemplate(CardTemplate model)
        {
            var entity = _repository.FirstOrDefault(s => s.id == model.id);
            if (entity == null || entity.id <= 0) return false;

            var time = entity.create_time;
            var oid = entity.create_openid;

            AutoMapper.Mapper.Map(model, entity);
            entity.create_time = time;
            entity.create_openid = oid;
            _repository.Update(entity);
            return uof.SaveChange() > 0;

        }
    }
}
