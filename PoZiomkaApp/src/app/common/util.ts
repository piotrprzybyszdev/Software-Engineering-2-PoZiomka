interface Array<T> {
  updateClone(index: number, value: T): Array<T>;
}

Array.prototype.updateClone = function<T>(index: number, value: T): T[] {
  return this.map((el, i) => i === index ? value : el);
}