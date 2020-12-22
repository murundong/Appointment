﻿using System;
using System.Collections.Generic;
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
    public class DoorsService : IDoorsService
    {
        public static IDbContextProvider<App_DbContext> _provider = new DbContextProvider<App_DbContext>();
        public IRepository<Doors> _repository = new RepositoryBase<App_DbContext, Doors>(_provider);
        public IRepository<UserInfos> _repositoryUInfos = new RepositoryBase<App_DbContext, UserInfos>(_provider);
        public IUnitOfWork uof = new UnitOfWork<App_DbContext, IDbContextProvider<App_DbContext>>(_provider);

        public Doors CreateDoors(Doors model)
        {
            _repository.Insert(model);
            if (uof.SaveChange() > 0) return model;
            return null;
        }

        public View_LessonDoorInfoOutput GetDoorInfo(int doorid)
        {
            View_LessonDoorInfoOutput res = new View_LessonDoorInfoOutput();
            var itemModel = _repository.FirstOrDefault(s => s.id == doorid);
            if(itemModel!=null && itemModel.id > 0)
            {
                var UModel = _repositoryUInfos.FirstOrDefault(s => s.open_id == itemModel.create_openid);
                if (!string.IsNullOrWhiteSpace(itemModel.door_banners)) res.banners = itemModel.door_banners.Split(',').ToList();
                res.door_name = itemModel.door_name;
                res.door_desc = itemModel.door_desc;
                if (UModel != null && UModel.uid > 0)
                {
                    res.door_manager = UModel.real_name ?? UModel.nick_name;
                    res.door_manager_img = UModel.avatar;
                }
                res.door_tel = itemModel.door_tel;
                res.door_address = itemModel.door_address;
                
            }
            return res;
        }

        public Base_PageOutput<List<View_TearcherDoorOutput>> GetDoors(View_DoorInput input)
        {
            Base_PageOutput<List<View_TearcherDoorOutput>> return_res = new Base_PageOutput<List<View_TearcherDoorOutput>>() {  data = new List<View_TearcherDoorOutput>()};
            if (input == null) input = new View_DoorInput();
            var query = _repository.GetAll().Where(s => s.active);
            return_res.total = query.Count();

            var query_end= query.OrderByDescending(s=>s.create_time)
                .Skip((input.page_index - 1) * input.page_size)
                .Take(input.page_size);
            var lst= AutoMapper.Mapper.Map<List<View_TearcherDoorOutput>>(query_end.ToList());
            return_res.data = lst;
            return return_res;
        }

        public Doors GetDoorsById(int id)
        {
            return _repository.FirstOrDefault(s => s.id == id);
        }

        public Base_PageOutput<List<View_TearcherDoorOutput>> GetTeacherDoors(View_TeacherDoorInput input)
        {
            Base_PageOutput<List<View_TearcherDoorOutput>> res = new Base_PageOutput<List<View_TearcherDoorOutput>>() { data = new List<View_TearcherDoorOutput>() };
            if (input == null || string.IsNullOrWhiteSpace(input.open_id)) return res;
            var query = _repository.GetAll()
                .Where(s => s.create_openid == input.open_id);
            res.total = query.Count();

            var query_end= query.OrderByDescending(s => s.create_time)
                 .Skip((input.page_index - 1) * input.page_size)
                .Take(input.page_size);
            res.data= AutoMapper.Mapper.Map<List<View_TearcherDoorOutput>>(query_end.ToList());
            return res;
        }

        public bool UpdateDoors(Doors model)
        {
            var entity = _repository.FirstOrDefault(s => s.id == model.id);
            entity.door_name = model.door_name;
            entity.door_desc = model.door_desc;
            entity.door_tel = model.door_tel;
            entity.door_img = model.door_img;
            entity.door_banners = model.door_banners;
            entity.door_address = model.door_address;
            entity.only_allow_member = model.only_allow_member;
            entity.status = model.status;
            entity.active = model.active;
            _repository.Update(entity);
            return uof.SaveChange() > 0;
        }
    }
}