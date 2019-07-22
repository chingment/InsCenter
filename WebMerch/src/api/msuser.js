import request from '@/utils/request'

export function fetchList(params) {
  return request({
    url: '/msuser/getlist',
    method: 'get',
    params
  })
}
