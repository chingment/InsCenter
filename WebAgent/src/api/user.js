import request from '@/utils/request'

export function fetchList(params) {
  return request({
    url: '/user/getlist',
    method: 'get',
    params
  })
}

export function addUser(data) {
  return request({
    url: '/user/add',
    method: 'post',
    data
  })
}

export function initEdit(params) {
  return request({
    url: '/user/initEdit',
    method: 'get',
    params
  })
}

export function editUser(data) {
  return request({
    url: '/user/edit',
    method: 'post',
    data
  })
}
