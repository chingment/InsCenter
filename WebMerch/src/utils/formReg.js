var reg = {
  userName: '^[0-9a-zA-Z_]{3,20}$',
  password: '^[0-9a-zA-Z_]{3,20}$',
  phoneNumber: /^((13[0-9])|(14[5|7])|(15([0-3]|[5-9]))|(18[0,5-9]))\d{8}$/,
  email: /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/
}

export default reg
