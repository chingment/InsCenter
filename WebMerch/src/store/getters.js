const getters = {
  sidebar: state => state.app.sidebar,
  device: state => state.app.device,
  token: state => state.own.token,
  avatar: state => state.own.avatar,
  name: state => state.own.name
}
export default getters
