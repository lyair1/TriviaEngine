'use strict';
module.exports = function(app) {
  var triviaController = require('../controllers/triviaController');

  app.route('/answer')
    .post(triviaController.findAnswer);
};