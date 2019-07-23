import request from '@/utils/request'

export function fetchList(params) {
  return request({
    url: '/user/getlist',
    method: 'get',
    params
  })
}
