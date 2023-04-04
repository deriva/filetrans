/**
 * **/
var Bind = {
	//绑定代理
	BindProxy: function(data) {
		var myProxy = new Proxy(data, {
			get: function(target, prop) {
				var it = Bind.GetKeyVal(target, prop);
				if (isNaN(it)) return it;
				else {
					var y = String(it).indexOf(".") + 1; //获取小数点的位置
					if (y > 0) {
						return parseFloat(it);
					} else {
						return parseInt(it);
					}
				}
			},
			set: function(target, prop, value) {
				//更新赋值 
				Bind.SetKeyVal(target, prop, value);
				//更新视图赋值 
				Bind.UpdateView(target, prop, value);

			},
			construct: function(target, argumentsList, newTarget) {
				return {
					value: argumentsList[0] * 10
				};
			},
			ownKeys(target) {
				return Reflect.ownKeys(target);
			}
		});
		return myProxy;
	},

	SetKeyVal: (target, prop, value) => {
		var dd = target;
		var keys = prop.split('.');
		keys.forEach(function(key) {
			if (dd.hasOwnProperty(key)) {
				var t1 = dd[key];
				if (typeof t1 != "object") {
					dd[key] = value;
				} else {
					dd = t1;
				}
			}
		});
	},
	GetKeyVal: (target, prop) => {
		var dd = target;
		var keys = prop.split('.');
		var val;
		keys.forEach(function(key) {
			if (dd.hasOwnProperty(key)) {
				var t1 = dd[key];
				if (typeof t1 != "object") {
					val = t1;
				} else {
					dd = t1;
					val = dd;
				}

			} else {//没有属性的话就返回对象
				//val = dd;
            }
		});
		return val;
	},
	/*
	更新视图
	*/
	UpdateView: (target, prop, value) => {
		$("[bf-model='" + prop + "']").each(function(i, dom) {
			var tag_name = $(dom)[0].nodeName.toLowerCase();
			var type = $(dom).attr("type");
			if (tag_name == "input" || tag_name == "textarea" || tag_name ==
				"select") {
				if (type != undefined && (type == "radio" || type == "checkbox")) {
					// $("[bf-model='" + prop + "'][value='" + value + "']").prop(
					// 	"checked", true);

				} else {
					$(dom).val(value);
				}
			} else {
				$(dom).html(value);
			}
		});


	},
	Register: function(proxy) {
		if ($("[bf-model]").length > 0) {
			$("[bf-model]").each(function(i, dom) {
				var type = $(dom).attr("type");
				var tag_name = $(dom)[0].nodeName.toLowerCase();
				if (tag_name === "input" || tag_name === "textarea" || tag_name === "select") {
					if (type != undefined && (type == "radio" || type == "checkbox")) {
						$(dom).unbind().bind("click", function(e) {
							var it = $(this).attr("bf-model");
							var val = [];
							if ($(this).prop("checked")) {
								val.push($(this).val());
							}
							proxy[it] = val.toString();
							if (typeof bindwatch != 'undefined') {
								bindwatch(it, val.toString());
							}
						});
					} else {
						$(dom).unbind().bind("input", function(e) {
							var it = $(this).attr("bf-model");
							proxy[it] = $(this).val();
							if (typeof bindwatch != 'undefined') {
								bindwatch(it, $(this).val());
							}
						});
						$(dom).unbind().bind("change", function (e) {
							var it = $(this).attr("bf-model");
							proxy[it] = $(this).val();
							if (typeof bindwatch != 'undefined') {
								bindwatch(it, $(this).val());
							}
						});
					}
				}
			});

		}
	},
	Reader: (target) => {

		$("[bf-model]").each(function(i, dom) {
			var prop = $(dom).attr("bf-model");
			var tag_name = $(dom)[0].nodeName.toLowerCase();
			var value = Bind.GetKeyVal(target, prop);
			var type = $(dom).attr("type");
			if (tag_name == "input" || tag_name == "textarea" || tag_name == "select") {
				if (type != undefined && (type == "radio" || type == "checkbox")) {
					$("[bf-model='" + prop + "'][value='" + value + "']").prop("checked", true);

				} else {
					$(dom).val(value);
				}
			} else {
				$(dom).html(value);
			}
		});


	},
	Config: () => {

	},
	Init: (data) => {
		var proxy = Bind.BindProxy(data);
		Bind.Register(proxy);
		Bind.Reader(data);
		return proxy;

	}
}
