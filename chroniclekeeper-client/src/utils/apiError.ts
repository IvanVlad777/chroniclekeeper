import axios from "axios";

/**
 * Izvuci čitljivu poruku iz API greške. Pokriva oblike koje backend vraća:
 * - ValidationProblemDetails: { errors: { Field: ["msg", ...] } }
 * - BadRequest(ModelState) / SerializableError: { Field: ["msg", ...] }
 * - Identity greške: [{ code, description }, ...]
 * - DomainValidationException i sl.: { message } ili čisti string
 */
export function apiErrorMessage(err: unknown, fallback: string): string {
    const data = axios.isAxiosError(err) ? err.response?.data : err;

    if (typeof data === "string" && data.trim()) return data;

    if (Array.isArray(data)) {
        const msgs = data
            .map((e) =>
                typeof e === "object" && e && "description" in e
                    ? String((e as { description: unknown }).description)
                    : String(e)
            )
            .filter(Boolean);
        if (msgs.length) return msgs.join(" ");
    }

    if (data && typeof data === "object") {
        const obj = data as Record<string, unknown>;

        if (obj.errors && typeof obj.errors === "object") {
            const msgs = Object.values(obj.errors as Record<string, unknown>)
                .flatMap((v) => (Array.isArray(v) ? v : [v]))
                .map(String)
                .filter(Boolean);
            if (msgs.length) return msgs.join(" ");
        }

        if (typeof obj.message === "string" && obj.message.trim()) {
            return obj.message;
        }

        // SerializableError: { Field: ["msg"] }
        const flat = Object.values(obj)
            .filter(Array.isArray)
            .flat()
            .map(String)
            .filter(Boolean);
        if (flat.length) return flat.join(" ");
    }

    return fallback;
}
