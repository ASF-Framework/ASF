﻿using ASF.Domain.Entities;
using ASF.Infrastructure.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ASF.Domain.Services
{
    public class PermissionChangeService
    {
        private readonly IPermissionRepository _permissionRepository;
        public PermissionChangeService(IPermissionRepository permissionRepository)
        {
            this._permissionRepository = permissionRepository;
        }
        /// <summary>
        /// 修改操作权限
        /// </summary>
        /// <param name="pid">权限标识</param>
        /// <param name="name">权限名称</param>
        /// <param name="parentId">分类</param>
        /// <param name="description">描述</param>
        /// <param name="enable">是否可用</param>
        /// <param name="apiAddress">api 地址</param>
        /// <param name="isLogger">是否记录日志</param>
        /// <returns></returns>
        public async Task<Result<Permission>> ModifyAction (string pid, string name, string parentId, string description, bool enable, string apiAddress, bool isLogger)
        {
            //获取权限数据
            var permission = await _permissionRepository.GetAsync(pid);
            if (permission == null)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);

            //如果是系统权限不能修改
            if (permission.IsSystem)
            {
                return Result<Permission>.ReFailure(ResultCodes.PermissionSystemNotModify.ToFormat(permission.Name));
            }

            //判断上级权限,上级权限不能为操作权限
            var paremt = await this._permissionRepository.GetAsync(parentId);
            if (paremt == null)
            {
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);
            }
            if (paremt.Type == Values.PermissionType.Action)
            {
                return Result<Permission>.ReFailure(ResultCodes.PermissionParemtNotAction);
            }

            permission.Name = name;
            permission.ParentId = parentId;
            permission.IsLogger = isLogger;
            permission.Enable = enable;
            permission.Description = description;
            permission.SetApiAddress(apiAddress);

            //验证权限聚合的数据合法性
            return permission.Valid<Permission>();
        }
        /// <summary>
        /// 修改导航权限
        /// </summary>
        /// <param name="pid">权限标识</param>
        /// <param name="name">权限名称</param>
        /// <param name="parentId">分类</param>
        /// <param name="description">描述</param>
        /// <param name="enable">是否可用</param>
        /// <param name="sort">导航排序</param>
        /// <returns></returns>
        public async Task<Result<Permission>> ModifyMenu(string pid, string name, string parentId,string description,bool enable, int sort)
        { 
            //获取权限数据
            var permission = await _permissionRepository.GetAsync(pid);
            if (permission == null)
                return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);

            //如果是系统权限不能修改
            if (permission.IsSystem)
            {
                return Result<Permission>.ReFailure(ResultCodes.PermissionSystemNotModify.ToFormat(permission.Name));
            }

            //如果配置上级权限，需要验证上级权限必须为导航权限
            if (permission.ParentId != parentId)
            {
                //判断上级权限,上级权限不能为导航权限
                var paremt = await this._permissionRepository.GetAsync(permission.ParentId);
                if (paremt == null)
                {
                    return Result<Permission>.ReFailure(ResultCodes.PermissionNotExist);
                }
                if (paremt.Type == Values.PermissionType.Action)
                {
                    return Result<Permission>.ReFailure(ResultCodes.PermissionParemtNotAction);
                }
            }

            permission.Name = name;
            permission.ParentId = parentId;
            permission.Sort = sort;
            permission.Enable = enable;
            permission.Description = description;

            //验证权限聚合的数据合法性
            return permission.Valid<Permission>();
        }



    }
}