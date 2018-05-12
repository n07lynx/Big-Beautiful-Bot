exports.array = {
	getRandomArray : function (array)
	{
		var arrayNo = Math.floor(array.length * Math.random());
		return array[arrayNo];
	}
};