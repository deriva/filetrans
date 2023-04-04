const _Authorization = 'token';
const _baseurl = '';
axios.defaults.timeout = 50000;
axios.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=UTF-8';
axios.defaults.baseURL = _baseurl;
var http = {
	baseURL: () => {
		return _baseurl;
	},
	getToken: function() {
		return localStorage.getItem("token");
	},

	/*
	  url
	  params请求后台的参数,如：{name:123,values:['a','b','c']}
	  loading是否显示遮罩层,可以传入true.false.及提示文本
	  config配置信息,如{timeout:3000,headers:{token:123}}
	*/
	post: function(url, params, config) {
		var layindex = 0;
		if (!config) { //默认不显示加载
			config = {};
			config.loading = true;
		}
		if (config.loading) {
			layindex = layer.msg('正在努力加载中......', {
				icon: 16,
				shade: 0.21
			});
		}
		//showLoading(loading);
		//axios.defaults.headers[_Authorization] = http.getToken();
		return new Promise((resolve, reject) => {
			axios.post(url, params, config)
				.then(response => {
					layer.close(layindex);
					resolve(response.data);
				}, err => {
					layer.close(layindex);
					reject(err && err.data && err.data.message ? err.data.message : '服务器处理异常');
				})
				.catch((error) => {
					layer.close(layindex);
					reject(error)
				})
		})
	},

	//=true异步请求时会显示遮罩层,=字符串，异步请求时遮罩层显示当前字符串
	get: function(url, param, config) {
		//showLoading(loading); 

		var layindex = 0;
		if(param != null && param != "" && typeof param == "object") {
			if(url.indexOf("?")==-1)url+="?";
			url += LP.ParseParams(param);
		}
		if (!config) { //默认不显示加载
			config = {};
			config.loading = true;
		}
		if (config.loading) {
			layindex = layer.msg('正在努力加载中......', {
				icon: 16,
				shade: 0.21
			});
		}
	//	axios.defaults.headers[_Authorization] = http.getToken();
		return new Promise((resolve, reject) => {
			axios.get(url, param)
				.then(response => {
					layer.close(layindex);
					resolve(response.data)
				}, err => {
					layer.close(layindex);
					reject(err)
				})
				.catch((error) => {
					layer.close(layindex);
					reject(error)
				})
		})
	}

}

//exports('http', http);
