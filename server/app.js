/**
 * 接收前端发送的服务器请求，并以JSON格式返回对应的结果。
 * 使用MongoDB数据库处理数据。
 * @author 杨耿聪
 */

var http = require('http');
var querystring = require('querystring');
var url = require('url');
var async = require('async');
var userModel = require('./Models/UserModel');
var itemModel = require('./Models/ItemModel');
var port = 8000;
var post;
var query;

http.createServer(onRequest).listen(port);

console.log('Listening at port ' + port);

// UserModel增加selectedItemId成员
function onRequest(request, response) {
	var pathname = url.parse(request.url).pathname;
	query = querystring.parse(url.parse(request.url).query);
	console.log("pathname: " + pathname);

	parsePost(request)
	.then(function() {
		switch (pathname) {
			case "/main":
				if (request.method == "GET")
					getUserInfo(request, response);
				else
					finishItem(request, response);
				break;
			case "/signup":
				signUp(request, response);
				break;
			case "/signin":
				signIn(request, response);
				break;
			case "/initItemInfo":
				getItemInfo(request, response);
				break;
			case "/deleteItem":
				deleteItem(request, response);
				break;
			case "/addItem":
				addItem(request, response);
				break;
			case "/friends":
				if (request.method == "GET")
					getFriends(request, response);
				else
					makeFriends(request, response);
				break;
			case "/finishedItem":
				getFinishedItems(request, response);
				break;
			case "/userInfo":
				updateUserInfo(request, response);
				break;
			default:
				response.write("The address does not exist!");
				response.end();
				break;
		}
	})
}

function parsePost(request) {
	return new Promise(function(resolve, reject){
		if (request.method == "POST") {
			post = "";
			request.on("data", function(chunk) {
				post += chunk;
			})
			.on("end", function() {
				post = querystring.parse(post);
				console.log("POST: ", post);
				resolve();
			});
		} else {
			resolve();
		}
    });
}

function getJsonResult(statusCode, list) {
	var jsonResult = {
		statusCode: statusCode,
		list: list
	};
	return JSON.stringify(jsonResult);
}

// 初始化的时候 GET 到 /main?userId=xxx 
// response返回对应该用户及其所有 friends 的， 所有未完成的 Item 信息
function getUserInfo(request, response) {
	console.log("Getting user information...");
	var list = [];
	var statusCode = "ok";
	var users = [];
	new Promise(function(resolve, reject) {
		userModel.findOne({userId: query.userId}, function(err, result) {
			if (result) {
				console.log("find out the user")
				if (result.friendsId) {
					users = result.friendsId.split('&');
				}
				resolve();
			} else {
				statusCode = "Can not find such an user!";
				reject();
			}
		})
	}).then(function() {
		console.log("finding the unfinished items");
		// 找出该用户及其好友未完成的item
		users.push(query.userId);
		async.forEach(users, function(user, callback) {
			itemModel.find({publisherId: user, isCompleted: "false"}, function(err, result) {
				if (err) {
						statusCode = err.message;
				} else {
					if (result.length != 0) {
						console.log("publisher: " + user);
						console.log("number of items: " + result.length);
						for (var j = 0; j < result.length; j++)
							list.push(result[j]);
					}
					callback();
				}
			})
		}, function() {
			list.sort(function(a, b) {
				return a.dateTime > b.dateTime ? 0 : 1;
			})
			response.write(getJsonResult(statusCode, list));
			response.end();
		})
	}, function() {
		response.write(getJsonResult(statusCode));
		response.end();
	})
}

// 当选择一个 Item 完成的时候 POST itemId到 /main?userId=xxx
// 需将item的consummatorId设为本用户
// 需将item的commission加到用户的score中
function finishItem(request, response) {
	console.log("Finishing item...");
	new Promise(function(resolve, reject) {
		itemModel.findOne({itemId: post.itemId}, function(err, result) {
			if (result) {
				result.isCompleted = "true";
				result.consummatorId = query.userId;
				result.save(function(err) {
					if (err) {
						response.write(getJsonResult(err.message));
						response.end();
					} else {
						resolve(result.commission);
					}
				})
			} else {
				response.write(getJsonResult("Can not find such an item!"));
				response.end();
			}
		})
	}).then(function(commission) {
		userModel.findOne({userId: query.userId}, function(err, result) {
			if (result) {
				console.log("score: " + result.score);
				result.score = parseInt(result.score) + parseInt(commission);
				console.log("current score: " + result.score);
				result.save(function(err) {
					if (err) {
						response.write(getJsonResult(err.message));
					} else {
						response.write(getJsonResult("ok"));
					}
					response.end();
				})
			} else {
				response.write(getJsonResult("Can not find the user!"));
				response.end();
			}
		})
	})
}

// 注册时 POST 到 /signup
// 检验注册的userName是否已存在，若不存在则返回值为 ok 的 status code ，否则返回错误信息
function signUp(request, response) {
	console.log("Signing up...");
	userModel.findOne({userName: post.userName}, function(err, result) {
		if (result) {
			response.write(getJsonResult("The user name has been registered!"));
			response.end();
		} else {
			userModel.create(post, function(err) {
				if (err) {
					response.write(getJsonResult(err.message));
				} else {
					console.log('create a new user!');
				response.write(getJsonResult("ok"));
				}
				response.end();
            });
		}
	})
}

// 登录时 POST 到 /signin
// 若无错误则返回用户数据，否则返回错误信息
function signIn(request, response) {
	console.log("Signing in...");
	userModel.findOne({userName: post.userName}, function(err, result) {
		if (result) {
			if (result.password == post.password) {
				response.write(getJsonResult("ok", result));
			} else {
				response.write(getJsonResult("Wrong password!"));
			}
		} else {
			response.write(getJsonResult("Can not find such an user!"));
		}
		response.end();
	})
}

// GET 到/initItemInfo?itemId=xxx
// response根据 itemId 返回通过该item的publisherId得到的userName以及各项信息
function getItemInfo(request, response) {
	console.log("Getting item information...");
	itemModel.findOne({itemId: query.itemId}, function(err, result) {
		if (result) {
			var itemInfo;
			userModel.findOne({userId: result.publisherId}, function(err2, publisher) {
				if (publisher) {
					itemInfo = {
						publisherName: publisher.userName,
						publisherId: result.publisherId,
						type: result.type,
						location: result.location,
						dateTime: result.dateTime,
						commission: result.commission,
						details: result.details
					}
					response.write(getJsonResult("ok", itemInfo));
				} else {
					response.write(getJsonResult("Can not find the publisher of the item!"));
				}
				response.end();
			})
		} else {
			response.write(getJsonResult("Can not find such an item!"));
			response.end();
		}
	})
}

// POST itemId到/deleteItem?userId=xxx
function deleteItem(request, response) {
	console.log("Deleting item...");
	itemModel.remove({itemId: post.itemId}, function(err) {
		if (err)
			response.write(getJsonResult(err.message));
		else
			response.write(getJsonResult("ok"));
		response.end();
	})
}

// 添加Item时 POST 到 /addItem?userId=xxx
// 注意更新对应user的itemId
// response返回status
function addItem(request, response) {
	console.log("Adding item...");
	new Promise(function(resolve, reject) {
		userModel.findOne({userId: query.userId}, function(err, result) {
			if (result) {
				if (result.itemId == "")
					result.itemId = post.itemId;
				else
					result.itemId += "&" + post.itemId;
				result.save(function(err) {
					if (err) {
						reject(err.message);
					} else {
						resolve();
					}
				});
			} else {
				reject("Can not find such an user!");
			}
		})
	}).then(function() {
		itemModel.create(post, function(err) {
			if (err) {
				response.write(getJsonResult(err.message));
			} else {
				console.log('create a new item!');
				response.write(getJsonResult("ok"));
			}
			response.end();
	    });
	}, function(arg) {
		response.write(getJsonResult(arg));
		response.end();
	})
}

// 初始化页面的时候需要 GET 到 /friends?userId=xxx
// response返回所有该用户的好友的 userName，头像 和 score
function getFriends(request, response) {
	console.log("Getting friends...");
	userModel.findOne({userId: query.userId}, function(err, result) {
		if (result) {
			var list = [];
			if (result.friendsId) {
				var friends = result.friendsId.split('&');
				new Promise(function(resolve, reject) {
					async.forEach(friends, function(friend, callback) {
						userModel.findOne({userId: friend}, function(err2, result2) {
							if (result2) {
								var temp = {
									userName: result2.userName,
									headSculpture: result2.headSculpture,
									score: result2.score
								}
								list.push(temp);
							}
							callback();
						})
					}, function() {
						list.sort(function(a, b) {
							return a.userName > b.userName ? 0 : 1;
						})
						resolve();
					});
				}).then(function() {
					response.write(getJsonResult("ok", list));
					response.end();
				});
			} else {
				response.write(getJsonResult("The user has no friend!"));
				response.end();
			}
		} else {
			response.write(getJsonResult("Can not find such an user!"));
			response.end();
		}
	})
}

// 新增好友的时候，需要 POST 好友的userName到 /friends?userId=xxx
// 注意更新好友的friendsId
function makeFriends(request, response) {
	console.log("Making friends...");
	userModel.findOne({userId: query.userId}, function(err, result) {
		if (result) {
			// 根据好友的userName找出好友的userId
			// 同时更新新好友的friendsId
			new Promise(function(resolve, reject) {
				userModel.findOne({userName: post.userName}, function(err2, newFriend) {
					if (newFriend) {
						// 不可加自己为好友
						if (newFriend.userId == query.userId) {
							reject("Can not make friends with yourself!");
						}
						// 不可重复加同一个好友
						var friends = newFriend.friendsId.split('&');
						var flag = true;
						for (var i = 0; i < friends.length; i++) {
							if (friends[i] == query.userId) {
								flag = false;
								break;
							}
						}
						if (!flag) {
							reject("Can not make friends again with one of your current friends!");
						} else {
							if (typeof(newFriend.friendsId) == "undefined" || newFriend.friendsId == "") {
								newFriend.friendsId = query.userId;
							} else {
								newFriend.friendsId += "&" + query.userId;
							}
							newFriend.save(function() {
								console.log("newFriend " + newFriend);
								resolve(newFriend.userId);
							})
						}
					} else {
						reject("Can not find such a user!");
					}
				})
			}).then(function(newFriendId) {
				if (typeof(result.friendsId) == "undefined" || result.friendsId == "") {
					result.friendsId = newFriendId;
				} else {
					result.friendsId += "&" + newFriendId;
				}
				result.save(function(err) {
					if (err) {
						response.write(getJsonResult(err.message));
					} else {
						response.write(getJsonResult("ok"));
					}
					response.end();
				})
			}, function(message) {
				response.write(getJsonResult(message));
				response.end();
			})
		} else {
			response.write(getJsonResult("Can not find such an user!"));
			response.end();
		}
	})
}

// 显示已完成的 Item, GET 到 /finishedItem?userId=xxx
function getFinishedItems(request, response) {
	console.log("Getting finished items...");
	itemModel.find({publisherId: query.userId, isCompleted: "true"}, function(err, result) {
		var list = [];
		var statusCode = "";
		if (result.length != 0) {
			console.log("finished items found: " + result);
			new Promise(function(res) {

				async.forEach(result, function(target, callback) {
					var item;
					var pName;
					var cName;
					new Promise(function(resolve, reject) {
						userModel.findOne({userId: target.publisherId}, function(err2, publisher) {
							if (publisher) {
								pName = publisher.userName;
								console.log("find publisher: " + pName);
							} else {
								statusCode += "Can not find the publisher of the item!";
							}
							resolve();
						})
					}).then(function(arg) {
						userModel.findOne({userId: target.consummatorId}, function(err3, consummator) {
							if (consummator) {
								cName = consummator.userName;
								console.log("find consummator: " + cName);
								if (pName) {
									item = {
										publisherName: pName,
										consummatorName: cName,
										itemId: target.itemId,
										commission: target.commission,
										location: target.location,
										type: target.type,
										headSculpture: target.headSculpture,
										dateTime: target.dateTime
									};
									list.push(item);
									statusCode = "ok";
								}
							} else {
								statusCode += "Can not find the consummator of the item!";
							}
							callback();
						})
					})
				}, function() {
					list.sort(function(a, b) {
						return a.dateTime > b.dateTime ? 0 : 1;
					})
					res();
				})

			}).then(function() {
				response.write(getJsonResult(statusCode, list));
				response.end();
			});
		} else {
			response.write(getJsonResult("No finished item or no such a user!"));
			response.end();
		}
	})
}

// 修改用户信息，POST 到 /userInfo?userId=xxx
function updateUserInfo(request, response) {
	console.log("Updating user information...");
	userModel.findOne({userId: query.userId}, function(err, result) {
		if (result) {
			for (var key in post) {
				result[key] = post[key];
			}
			result.save(function(err) {
				if (err) {
					response.write(getJsonResult(err.message));
				} else {
					response.write(getJsonResult("ok"));
				}
				response.end();
			})
		} else {
			response.write(getJsonResult("Can not find such an user!"));
			response.end();
		}
	})
}
