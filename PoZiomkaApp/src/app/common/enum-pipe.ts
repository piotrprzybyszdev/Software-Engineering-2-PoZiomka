import { KeyValuePipe } from "@angular/common";
import { KeyValueDiffers, Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'enum'
})
export class EnumPipe implements PipeTransform {
    pipe: KeyValuePipe;

    constructor(differs: KeyValueDiffers) {
        this.pipe = new KeyValuePipe(differs);
    }

    transform<K extends string, V>(input: Record<K, V> | ReadonlyMap<K, V>): Array<V> {
        return this.pipe.transform(input).filter(v => !isNaN(Number(v.value))).map(v => v.value);
    }
}