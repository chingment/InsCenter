import request from '@/utils/request'

export function loginByAccount(data) {
  return request({
    url: '/own/loginByAccount',
    method: 'post',
    data
  })
}

export function getInfo(token) {
  return request({
    url: '/own/getInfo',
    method: 'get',
    params: { token }
  })
}

export function logout(token) {
  return request({
    url: '/own/logout',
    method: 'post',
    params: { token }
  })
}

export function checkPermission(data) {
  return request({
    url: '/own/checkPermission',
    method: 'post',
    data
  })
}
