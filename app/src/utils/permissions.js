export function actionToObject (json) {
  try {
    return JSON.parse(json)
  } catch (e) {
    .log('err', e.message)
  }
  return []
}
