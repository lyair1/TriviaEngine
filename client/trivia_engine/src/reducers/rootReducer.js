import {combineReducers} from 'redux';
import initialState from './initialState';
import * as types from './../actions/actionsTypes';

function questions(state = initialState.questions, action) {
    let newState = JSON.parse(JSON.stringify(state));
    switch (action.type) {
      case types.GET_ANSWER:
        return action;
      case types.ANSWER_RECEIVED:
        newState.push(action.payload);
        return newState;
      default:
        return state;
    }
  }

const rootReducer = combineReducers({
  questions
});

export default rootReducer;