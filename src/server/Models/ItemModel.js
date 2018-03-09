/**
 * 定义Item模型
 * @author 杨耿聪
 */

var mongoose = require('./ConnectedDB');

var itemSchema = mongoose.Schema({
	publisherId: String,
	consummatorId: String,
	itemId: String,
	type: String,
	details: String,
	location: String,
	dateTime: String,
	image: String,
	isCompleted: String,
	commission: String
}); 

var itemModel = mongoose.model('Item', itemSchema);

module.exports = itemModel;