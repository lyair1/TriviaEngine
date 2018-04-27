import * as types from './actionsTypes';

export function receiveAnswer(json) {
  return {type: types.ANSWER_RECEIVED, payload: json};
}

export function fetchStuff(data) {
  console.log(JSON.stringify(data));
  return dispatch => {
    dispatch({type: types.START_FETCH});
    return fetch('/Trivia/AnswerQuestion', {
      method: 'POST',
      headers: new Headers({
        'Content-Type': 'application/json',
        Accept: 'application/json',
      }),
      body: JSON.stringify(data)
    })
    .then(response => {
        dispatch({type: types.END_FETCH});
        let json = response ? response.json() : {};
        return json;
    })
    .then(json => {
        console.log(json)
        dispatch(receiveAnswer(json))
      }).catch((error) => {
        console.log(error);
      });
  };
}