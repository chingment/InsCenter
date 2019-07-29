import request from '@/utils/request'

export function loginByAccount(data) {
  return request({
    url: '/own/loginByAccount',
    method: 'post',
    data
  })
}

export function getInfo(token, website) {
  return request({
    url: '/own/getInfo',
    method: 'get',
    params: { token, website }
  })
}

export function logout(token) {
  return request({
    url: '/own/logout',
    method: 'post',
    params: { token }
  })
}

export function checkPermission(code) {
  return request({
    url: '/own/checkPermission',
    method: 'get',
    params: { code }
  })
}
